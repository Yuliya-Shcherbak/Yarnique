using Autofac;
using Serilog;
using Yarnique.Common.Application;
using Yarnique.Common.Infrastructure;
using Yarnique.Common.Infrastructure.EventBus;
using Yarnique.Modules.Designs.Application.DesignCreation.PublishDesign;
using Yarnique.Modules.Designs.Infrastructure.Configuration.DataAccess;
using Yarnique.Modules.Designs.Infrastructure.Configuration.EventsBus;
using Yarnique.Modules.Designs.Infrastructure.Configuration.Mediator;
using Yarnique.Modules.Designs.Infrastructure.Configuration.Processing;
using Yarnique.Modules.Designs.Infrastructure.Configuration.Processing.Outbox;
using Yarnique.Modules.Designs.Infrastructure.Configuration.Quartz;

namespace Yarnique.Modules.Designs.Infrastructure.Configuration
{
    public class DesignsStartup
    {
        private static IContainer _container;

        public static void Initialize(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            IEventsBus eventsBus,
            bool inTest = false,
            long? internalProcessingPoolingInterval = null)
        {
            var moduleLogger = logger.ForContext("Module", "Designs");

            ConfigureCompositionRoot(
                connectionString,
                executionContextAccessor,
                logger,
                eventsBus);

            QuartzStartup.Initialize(moduleLogger, inTest, internalProcessingPoolingInterval);

            EventsBusStartup.Initialize(moduleLogger);
        }

        public static void Stop()
        {
            QuartzStartup.StopQuartz();
        }

        private static void ConfigureCompositionRoot(
            string connectionString,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            IEventsBus eventsBus)
        {
            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new LoggingModule(logger.ForContext("Module", "Designs")));

            var loggerFactory = new Serilog.Extensions.Logging.SerilogLoggerFactory(logger);
            containerBuilder.RegisterModule(new DataAccessModule(connectionString, loggerFactory));
            containerBuilder.RegisterModule(new ProcessingModule());
            containerBuilder.RegisterModule(new EventsBusModule(eventsBus));
            containerBuilder.RegisterModule(new MediatorModule());

            BiDictionary<string, Type> domainNotificationsMap = new BiDictionary<string, Type>();
            domainNotificationsMap.Add("DesignPublishedNotification", typeof(DesignPublishedNotification));
            containerBuilder.RegisterModule(new OutboxModule(domainNotificationsMap));

            containerBuilder.RegisterModule(new QuartzModule());

            containerBuilder.RegisterInstance(executionContextAccessor);

            _container = containerBuilder.Build();

            DesignsCompositionRoot.SetContainer(_container);
        }
    }
}
