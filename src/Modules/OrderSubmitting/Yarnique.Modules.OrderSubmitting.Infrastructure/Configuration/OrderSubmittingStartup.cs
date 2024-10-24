using Autofac;
using Serilog;
using Yarnique.Common.Application;
using Yarnique.Common.Infrastructure;
using Yarnique.Common.Infrastructure.EventBus;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.DataAccess;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.EventsBus;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.Mediator;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.Processing;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.Processing.Outbox;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.Quartz;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration
{
    public class OrderSubmittingStartup
    {
        private static IContainer _container;

        public static void Initialize(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            IEventsBus eventsBus,
            long? internalProcessingPoolingInterval = null)
        {
            var moduleLogger = logger.ForContext("Module", "OrderSubmitting");

            ConfigureCompositionRoot(
                connectionString,
                executionContextAccessor,
                logger,
                eventsBus);

            QuartzStartup.Initialize(moduleLogger, internalProcessingPoolingInterval);

            EventsBusStartup.Initialize(moduleLogger);
        }

        private static void ConfigureCompositionRoot(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            IEventsBus eventsBus)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "OrderSubmitting")));

            var loggerFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory(logger);
            containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
            containerBuilder.RegisterModule(new ProcessingModule());
            containerBuilder.RegisterModule(new EventsBusModule(eventsBus));
            containerBuilder.RegisterModule(new MediatorModule());

            containerBuilder.RegisterModule(new OutboxModule(new BiDictionary<string, Type>()));

            containerBuilder.RegisterModule(new QuartzModule());

            containerBuilder.RegisterInstance(executionContextAccessor);

            _container = containerBuilder.Build();

            OrderSubmittingCompositionRoot.SetContainer(_container);
        }
    }
}
