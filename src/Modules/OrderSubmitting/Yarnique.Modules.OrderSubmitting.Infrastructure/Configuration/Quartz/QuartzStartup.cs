using Quartz.Impl;
using Quartz.Logging;
using Quartz;
using System.Collections.Specialized;
using Serilog;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.Processing.Outbox;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.Processing.InternalCommands;
using Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.Processing.Inbox;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration.Quartz
{
    internal static class QuartzStartup
    {
        internal static void Initialize(ILogger logger, bool inTest = false, long? internalProcessingPoolingInterval = null)
        {
            logger.Information("Quartz starting...");

            var schedulerConfiguration = new NameValueCollection();
            var instanceName = inTest ? $"OrderSubmitting-{Guid.NewGuid()}" : "OrderSubmitting";
            schedulerConfiguration.Add("quartz.scheduler.instanceName", instanceName);

            ISchedulerFactory schedulerFactory = new StdSchedulerFactory(schedulerConfiguration);
            IScheduler scheduler = schedulerFactory.GetScheduler().GetAwaiter().GetResult();

            LogProvider.SetCurrentLogProvider(new SerilogLogProvider(logger));

            scheduler.Start().GetAwaiter().GetResult();

            var processOutboxJob = JobBuilder.Create<ProcessOutboxJob>().Build();
            ITrigger trigger;
            if (internalProcessingPoolingInterval.HasValue)
            {
                trigger =
                    TriggerBuilder
                        .Create()
                        .StartNow()
                        .WithSimpleSchedule(x =>
                            x.WithInterval(TimeSpan.FromMilliseconds(internalProcessingPoolingInterval.Value))
                                .RepeatForever())
                        .Build();
            }
            else
            {
                trigger =
                    TriggerBuilder
                        .Create()
                        .StartNow()
                        .WithCronSchedule("0/5 * * ? * *")
                        .Build();
            }

            scheduler
                .ScheduleJob(processOutboxJob, trigger)
                .GetAwaiter().GetResult();

            var processInboxJob = JobBuilder.Create<ProcessInboxJob>().Build();

            ITrigger processInboxTrigger;
            if (internalProcessingPoolingInterval.HasValue)
            {
                processInboxTrigger =
                    TriggerBuilder
                        .Create()
                        .StartNow()
                        .WithSimpleSchedule(x =>
                            x.WithInterval(TimeSpan.FromMilliseconds(internalProcessingPoolingInterval.Value))
                                .RepeatForever())
                        .Build();
            }
            else
            {
                processInboxTrigger =
                    TriggerBuilder
                        .Create()
                        .StartNow()
                        .WithCronSchedule("0/5 * * ? * *")
                        .Build();
            }

            scheduler
                .ScheduleJob(processInboxJob, processInboxTrigger)
                .GetAwaiter().GetResult();

            var processInternalCommandsJob = JobBuilder.Create<ProcessInternalCommandsJob>().Build();

            ITrigger processInternalCommandsTrigger;
            if (internalProcessingPoolingInterval.HasValue)
            {
                processInternalCommandsTrigger =
                    TriggerBuilder
                        .Create()
                        .StartNow()
                        .WithSimpleSchedule(x =>
                            x.WithInterval(TimeSpan.FromMilliseconds(internalProcessingPoolingInterval.Value))
                                .RepeatForever())
                        .Build();
            }
            else
            {
                processInternalCommandsTrigger =
                    TriggerBuilder
                        .Create()
                        .StartNow()
                        .WithCronSchedule("0/5 * * ? * *")
                        .Build();
            }

            scheduler.ScheduleJob(processInternalCommandsJob, processInternalCommandsTrigger).GetAwaiter().GetResult();

            logger.Information("Quartz started.");
        }
    }
}
