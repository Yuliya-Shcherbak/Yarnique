using Autofac;
using Microsoft.AspNetCore.SignalR;
using Serilog;
using Yarnique.Common.Application;
using Yarnique.Common.Infrastructure;
using Yarnique.Common.Infrastructure.EventBus;
using Yarnique.Modules.OrderSubmitting.Application.Callbacks;
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
            string paymentApiUrl,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            IEventsBus eventsBus,
            IHubContext<OrderStatusHub, IOrderStatusHub> hubContext = null,
            bool inTest = false,
            long? internalProcessingPoolingInterval = null)
        {
            var moduleLogger = logger.ForContext("Module", "OrderSubmitting");

            ConfigureCompositionRoot(
                connectionString,
                paymentApiUrl,
                executionContextAccessor,
                logger,
                eventsBus,
                hubContext);

            QuartzStartup.Initialize(moduleLogger, inTest, internalProcessingPoolingInterval);

            EventsBusStartup.Initialize(moduleLogger);
        }

        private static void ConfigureCompositionRoot(
            string connectionString,
            string paymentApiUrl,
            IExecutionContextAccessor executionContextAccessor,
            ILogger logger,
            IEventsBus eventsBus,
            IHubContext<OrderStatusHub, IOrderStatusHub> hubContext = null)
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

            containerBuilder.RegisterModule(new OrderPaymentHttpClientModule(paymentApiUrl));

            if (hubContext != null) containerBuilder.RegisterModule(new CallbackModule(hubContext));

            containerBuilder.RegisterInstance(executionContextAccessor);
            _container = containerBuilder.Build();

            OrderSubmittingCompositionRoot.SetContainer(_container);
        }
    }
}
