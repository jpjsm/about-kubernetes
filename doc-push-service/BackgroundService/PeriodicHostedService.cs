using doc_push_service.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace doc_push_service.BackgroundServices
{
    
    class PeriodicHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _factory;
        private readonly ILogger<PeriodicHostedService> _logger;
        private readonly TimeSpan _period = TimeSpan.FromSeconds(90);
        private int _executionCount = 0;
        public bool IsEnabled { get; set; }

        public PeriodicHostedService(ILogger<PeriodicHostedService> logger, IServiceScopeFactory factory)
        {
            _logger = logger ?? NullLogger<PeriodicHostedService>.Instance;
            _factory = factory;
            IsEnabled = true;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using PeriodicTimer timer = new PeriodicTimer(_period);
            while (
                !stoppingToken.IsCancellationRequested &&
                await timer.WaitForNextTickAsync(stoppingToken))
            {
                try
                {
                    if (IsEnabled)
                    {
                        await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
                        DocPusherService pushService = asyncScope.ServiceProvider.GetRequiredService<DocPusherService>();
                        await pushService.PushDocumentsAsync();
                        _executionCount++;
                        _logger.LogInformation(
                            $"Executed PeriodicHostedService - Count: {_executionCount}");
                    }
                    else
                    {
                        _logger.LogInformation(
                            "Skipped PeriodicHostedService");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(
                        $"Failed to execute PeriodicHostedService with exception message {ex.Message}. Good luck next round!");
                }
            }
        }
    }
}