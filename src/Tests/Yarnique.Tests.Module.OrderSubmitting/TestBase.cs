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
            const string connectionStringEnvironmentVariable = "TestDataBase_ConnectionString";
            ConnectionString = EnvironmentVariablesProvider.GetVariable(connectionStringEnvironmentVariable);
            if (ConnectionString == null)
            {
                throw new ApplicationException(
                    $"Define connection string to Order Submitting module database using environment variable: {connectionStringEnvironmentVariable}");
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
        }

        internal async Task<(Guid designId, List<Guid> designPartIds, List<Guid> designPartSpecificationIds)> AddDesign()
        {
            var faker = new Faker("en");
            var designId = Guid.NewGuid();
            var designPartIds = new List<Guid>() { Guid.NewGuid(), Guid.NewGuid() };
            var designPartSpecificationIds = new List<Guid>();

            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                foreach (var id in designPartIds)
                {
                    await sqlConnection.ExecuteScalarAsync("INSERT INTO [orders].[DesignParts] VALUES (@Id, 'Pretty Tie') ", new { Id = id, Name = $"DP-{Guid.NewGuid()}" });
                }

                await sqlConnection.ExecuteScalarAsync("INSERT INTO [orders].[Designs] VALUES (@Id, 'Tiny Rabbit', 120, 0, @SellerId) ", new { Id = designId, SellerId = Guid.NewGuid() });

                foreach (var id in designPartIds)
                {
                    var dpsId = Guid.NewGuid();
                    await sqlConnection.ExecuteScalarAsync("INSERT INTO [orders].[DesignPartSpecifications] VALUES (@Id, @DesignId, @DesignPartId, 80, '1.00:00:00', 1) ",
                        new { Id = dpsId, DesignId = designId, DesignPartId = id });
                    designPartSpecificationIds.Add(dpsId);
                }
            }

            return (designId, designPartIds, designPartSpecificationIds);
        }
    }
}
