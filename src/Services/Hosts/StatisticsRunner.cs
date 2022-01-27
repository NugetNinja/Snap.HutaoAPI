using Microsoft.Extensions.Caching.Memory;

namespace Snap.Genshin.Website.Services.Hosts
{
    public class StatisticsRunner : BackgroundService
    {
        public StatisticsRunner(GenshinStatisticsService statisticsService, IMemoryCache cache, ILogger<StatisticsRunner> logger)
        {
            this.statisticsService = statisticsService;
            this.cache = cache;
            this.logger = logger;
        }

        private readonly GenshinStatisticsService statisticsService;
        private readonly IMemoryCache cache;
        private readonly ILogger logger;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (DateTime.UtcNow.Hour != 18)
                {
                    // 等待一小时
                    await Task.Delay(new TimeSpan(1, 0, 0), stoppingToken);
                    continue;
                }

                logger.LogInformation("已触发数据处理...");

                // 写入忙碌标识
                cache.Set("_STATISTICS_BUSY", true);

                // 计算数据
                await this.statisticsService.CaltulateStatistics();

                // 清除忙碌标识
                cache.Remove("_STATISTICS_BUSY");
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("数据统计服务已启动。");
            return Task.CompletedTask;
        }
    }
}
