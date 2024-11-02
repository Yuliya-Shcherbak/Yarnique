using Autofac;
using Serilog;
using Yarnique.Common.Application;
using Yarnique.Common.Infrastructure;
using Yarnique.Modules.UsersManagement.Domain.Identity;
using Yarnique.Modules.UsersManagement.Infrastructure.Configuration.DataAccess;
using Yarnique.Modules.UsersManagement.Infrastructure.Configuration.Mediator;
using Yarnique.Modules.UsersManagement.Infrastructure.Configuration.Processing;
using Yarnique.Modules.UsersManagement.Infrastructure.Configuration.Processing.Outbox;

namespace Yarnique.Modules.UsersManagement.Infrastructure.Configuration
{
    public class UsersManagementStartup
    {
        private static IContainer _container;

        public static void Initialize(
            string connectionString,
            IdentityConfig identityConfig,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            long? internalProcessingPoolingInterval = null)
        {
            var moduleLogger = logger.ForContext("Module", "UsersManagement");

            ConfigureCompositionRoot(
                connectionString,
                identityConfig,
                executionContextAccessor,
                logger
                );
        }

        private static void ConfigureCompositionRoot(
            string connectionString,
            IdentityConfig identityConfig,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger
            )
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "UsersManagement")));

            var loggerFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory(logger);
            containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
            containerBuilder.RegisterModule(new ProcessingModule());
            containerBuilder.RegisterModule(new MediatorModule());
            containerBuilder.RegisterModule(new OutboxModule(new BiDictionary<string, Type>()));
            containerBuilder.RegisterModule(new TokenModule(identityConfig));

            containerBuilder.RegisterInstance(executionContextAccessor);

            _container = containerBuilder.Build();

            UsersManagementCompositionRoot.SetContainer(_container);
        }
    }
}
