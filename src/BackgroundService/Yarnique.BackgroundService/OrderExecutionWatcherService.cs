using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;
using Yarnique.BackgroundService.Helpers;

namespace Yarnique.BackgroundService
{
    public class OrderExecutionWatcherService : IHostedService
    {
        private readonly IProcessOrderService _helper;
        private readonly ILogger<OrderExecutionWatcherService> _logger;
        private readonly string _cronExpression;
        private CronExpression _cronSchedule;
        private Timer _timer;
        private readonly AsyncRetryPolicy _retryPolicy;

        public OrderExecutionWatcherService(IProcessOrderService helper, ILogger<OrderExecutionWatcherService> logger)
        {
            _helper = helper;
            _logger = logger;
            _cronExpression = "0 8 * * *";
            _cronSchedule = CronExpression.Parse(_cronExpression);

            _retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 3,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timeSpan, retryAttempt, context) =>
                {
                    _logger.LogWarning(exception, "Attempt {RetryAttempt} - Retrying in {TimeSpan}", retryAttempt, timeSpan);
                });
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ScheduleNextRun();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void ScheduleNextRun()
        {
            var next = _cronSchedule.GetNextOccurrence(DateTime.UtcNow, true);
            if (next.HasValue)
            {
                var delay = next.Value - DateTime.UtcNow;
                _timer = new Timer(ExecuteTask, null, delay, Timeout.InfiniteTimeSpan);
            }
        }

        private async void ExecuteTask(object state)
        {
            _logger.LogInformation("The service started ExecuteTask at: {time}", DateTimeOffset.Now);

            try
            {
                await _retryPolicy.ExecuteAsync(_helper.ProcessDueOrders);
                _logger.LogInformation("The service finished ExecuteTask at: {time}", DateTimeOffset.Now);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email notification after multiple retries");
            }

            ScheduleNextRun();
        }
    }
}
