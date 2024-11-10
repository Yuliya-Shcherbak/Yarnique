using Autofac;
using Dapper;
using Serilog;
using System.Data.SqlClient;
using Yarnique.Common.Infrastructure.EventBus;
using Yarnique.Modules.OrderSubmitting.Application.Contracts;
using Yarnique.Modules.OrderSubmitting.Infrastructure;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration;
using Yarnique.Test.Common;

namespace Yarnique.Tests.Module.OrderSubmitting
{
    public class TestBase
    {
        protected string ConnectionString { get; private set; }
        protected ILogger _logger { get; private set; }
        protected IOrderSubmittingModule _ordersModule { get; private set; }
        protected ExecutionContextMock _executionContext { get; private set; }

        protected TestBase()
        {
            const string connectionStringEnvironmentVariable = "ASPNETCORE_IntegrationTests_ConnectionString";
            ConnectionString = EnvironmentVariablesProvider.GetVariable(connectionStringEnvironmentVariable);
            if (ConnectionString == null)
            {
                throw new ApplicationException(
                    $"Define connection string to integration tests database using environment variable: {connectionStringEnvironmentVariable}");
            }

            _logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            _executionContext = new ExecutionContextMock(Guid.NewGuid());

            var builder = new ContainerBuilder();
            builder.RegisterModule(new EventsBusModuleMock(_logger));
            var container = builder.Build();
            var eventsBus = container.Resolve<IEventsBus>();

            OrderSubmittingStartup.Initialize(
                ConnectionString,
                string.Empty,
                _executionContext,
                _logger,
                eventsBus,
                true);

            _ordersModule = new OrderSubmittingModule();

            ClearDatabase();
        }

        internal async Task AddDesign(Guid designId)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                var designPartIdFirst = Guid.NewGuid();
                var designPartIdSecond = Guid.NewGuid();

                await sqlConnection.ExecuteScalarAsync("INSERT INTO [orders].[DesignParts] VALUES (@Id, 'Pretty Tie') ", new { Id = designPartIdFirst });
                await sqlConnection.ExecuteScalarAsync("INSERT INTO [orders].[DesignParts] VALUES (@Id, 'Pretty Bow') ", new { Id = designPartIdSecond });
                await sqlConnection.ExecuteScalarAsync("INSERT INTO [orders].[Designs] VALUES (@Id, 'Tiny Rabbit', 120, 0) ", new { Id = designId });
                await sqlConnection.ExecuteScalarAsync("INSERT INTO [orders].[DesignPartSpecifications] VALUES (@Id, @DesignId, @DesignPartId, 80, '1.00:00:00') ",
                    new { Id = Guid.NewGuid(), DesignId = designId, DesignPartId = designPartIdFirst });
                await sqlConnection.ExecuteScalarAsync("INSERT INTO [orders].[DesignPartSpecifications] VALUES (@Id, @DesignId, @DesignPartId, 50, '1.00:00:00') ",
                    new { Id = Guid.NewGuid(), DesignId = designId, DesignPartId = designPartIdSecond });
            }
        }

        private void ClearDatabase()
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                const string sql =
                @"
                DELETE FROM [users].[Users]
                DELETE FROM [orders].[InboxMessages]
                DELETE FROM [orders].[InternalCommands]
                DELETE FROM [orders].[OutboxMessages]
                DELETE FROM [orders].[DesignPartSpecifications]
                DELETE FROM [orders].[DesignParts]
                DELETE FROM [orders].[Orders]
                DELETE FROM [orders].[Designs]
                ";

                sqlConnection.ExecuteScalar(sql);
            }
        }
    }
}