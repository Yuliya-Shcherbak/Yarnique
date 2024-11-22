using Autofac;
using Dapper;
using Serilog;
using System.Data.SqlClient;
using Yarnique.Common.Infrastructure.EventBus;
using Yarnique.Modules.Designs.Application.Contracts;
using Yarnique.Modules.Designs.Infrastructure;
using Yarnique.Modules.Designs.Infrastructure.Configuration;
using Yarnique.Modules.OrderSubmitting.Application.Contracts;
using Yarnique.Modules.OrderSubmitting.Infrastructure;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration;
using Yarnique.Test.Common;
using Yarnique.Test.Common.Probing;

namespace Yarnique.Test.Integration.PublishDesign
{
    public class TestBase
    {
        protected string ConnectionString { get; private set; }
        protected ILogger _logger { get; private set; }
        protected IDesignsModule _designsModule { get; private set; }
        protected IOrderSubmittingModule _ordersModule { get; private set; }
        protected ExecutionContextMock _executionContext { get; private set; }

        protected TestBase()
        {
            const string connectionStringEnvironmentVariable = "TestDataBase_ConnectionString";
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

            DesignsStartup.Initialize(
                ConnectionString,
                _executionContext,
                _logger,
                eventsBus,
                true);

            _ordersModule = new OrderSubmittingModule();
            _designsModule = new DesignsModule();

            ClearDatabase();
        }

        protected static async Task AssertEventually(IProbe probe, int timeout)
        {
            await new Poller(timeout).CheckAsync(probe);
        }

        private void ClearDatabase()
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                const string sql =
                @"
                DELETE FROM [designs].[InboxMessages]
                DELETE FROM [designs].[InternalCommands]
                DELETE FROM [designs].[OutboxMessages]
                DELETE FROM [designs].[DesignPartSpecifications]
                DELETE FROM [designs].[Designs]
                DELETE FROM [designs].[DesignParts]
                DELETE FROM [orders].[InboxMessages]
                DELETE FROM [orders].[InternalCommands]
                DELETE FROM [orders].[OutboxMessages]
                DELETE FROM [orders].[DesignPartSpecifications]
                DELETE FROM [orders].[DesignParts]
                DELETE FROM [orders].[Orders]
                DELETE FROM [orders].[Designs]
                DELETE FROM [users].[Users]
                ";

                sqlConnection.ExecuteScalar(sql);
            }
        }
    }
}
