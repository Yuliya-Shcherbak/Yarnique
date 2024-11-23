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
    }
}
