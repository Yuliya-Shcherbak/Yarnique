using Autofac;
using Azure.Storage.Blobs;
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
                new BlobServiceClient("UseDevelopmentStorage=true"),
                eventsBus,
                true);

            _ordersModule = new OrderSubmittingModule();
            _designsModule = new DesignsModule();
        }

        protected static async Task AssertEventually(IProbe probe, int timeout)
        {
            await new Poller(timeout).CheckAsync(probe);
        }

        public void CleanUp(
            Guid[] designPartIds,
            Guid[] designPartSpecificationIds,
            Guid[] designIds,
            Guid[] userIds)
        {
            using (var sqlConnection = new SqlConnection(ConnectionString))
            {
                const string deleteMessagesSql =
                @"
                DELETE FROM [designs].[InboxMessages]
                DELETE FROM [designs].[InternalCommands]
                DELETE FROM [designs].[OutboxMessages]
                DELETE FROM [orders].[InboxMessages]
                DELETE FROM [orders].[InternalCommands]
                DELETE FROM [orders].[OutboxMessages]
                ";
                sqlConnection.ExecuteScalar(deleteMessagesSql);

                if (designPartSpecificationIds.Length > 0)
                {
                    sqlConnection.ExecuteScalar("DELETE FROM [designs].[DesignPartSpecifications] WHERE Id in @designPartSpecificationIds", new { designPartSpecificationIds });
                    sqlConnection.ExecuteScalar("DELETE FROM [orders].[DesignPartSpecifications] WHERE Id in @designPartSpecificationIds", new { designPartSpecificationIds });
                }

                if (designPartIds.Length > 0)
                {
                    sqlConnection.ExecuteScalar("DELETE FROM [designs].[DesignParts] WHERE Id in @designPartIds", new { designPartIds });
                    sqlConnection.ExecuteScalar("DELETE FROM [orders].[DesignParts] WHERE Id in @designPartIds", new { designPartIds });
                }

                if (designIds.Length > 0)
                {
                    sqlConnection.ExecuteScalar("DELETE FROM [designs].[Designs] WHERE Id in @designIds", new { designIds });
                    sqlConnection.ExecuteScalar("DELETE FROM [orders].[Designs] WHERE Id in @designIds", new { designIds });
                }

                if (userIds.Length > 0)
                {
                    sqlConnection.ExecuteScalar("DELETE FROM [users].[Users] WHERE Id in @userIds", new { userIds });
                }
            }
        }
    }
}
