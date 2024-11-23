using Autofac;
using Dapper;
using Serilog;
using System.Data.SqlClient;
using Yarnique.Common.Infrastructure.EventBus;
using Yarnique.Modules.Designs.Application.Contracts;
using Yarnique.Modules.Designs.Infrastructure;
using Yarnique.Modules.Designs.Infrastructure.Configuration;
using Yarnique.Test.Common;


namespace Yarnique.Test.Module.Designs
{
    public abstract partial class TestBase : IDisposable
    {
        protected string ConnectionString { get; private set; }
        protected ILogger _logger { get; private set; }
        protected IDesignsModule _designsModule { get; private set; }
        protected ExecutionContextMock _executionContext { get; private set; }

        protected TestBase()
        {
            const string connectionStringEnvironmentVariable = "TestDataBase_ConnectionString";
            ConnectionString = EnvironmentVariablesProvider.GetVariable(connectionStringEnvironmentVariable);
            if (ConnectionString == null)
            {
                throw new ApplicationException(
                    $"Define connection string to Designs Module tests database using environment variable: {connectionStringEnvironmentVariable}");
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

            DesignsStartup.Initialize(
                ConnectionString,
                _executionContext,
                _logger,
                eventsBus,
                true);

            _designsModule = new DesignsModule();
        }

        public void Dispose()
        {
            DesignsStartup.Stop();
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
                ";
                sqlConnection.ExecuteScalar(deleteMessagesSql);

                if (designPartSpecificationIds.Length > 0)
                {
                    sqlConnection.ExecuteScalar("DELETE FROM [designs].[DesignPartSpecifications] WHERE Id in @designPartSpecificationIds", new { designPartSpecificationIds });
                }

                if (designPartIds.Length > 0)
                {
                    sqlConnection.ExecuteScalar("DELETE FROM [designs].[DesignParts] WHERE Id in @designPartIds", new { designPartIds });
                }

                if (designIds.Length > 0)
                {
                    sqlConnection.ExecuteScalar("DELETE FROM [designs].[Designs] WHERE Id in @designIds", new { designIds });
                }

                if (userIds.Length > 0)
                {
                    sqlConnection.ExecuteScalar("DELETE FROM [users].[Users] WHERE Id in @userIds", new { userIds });
                }
            }
        }
    }
}
