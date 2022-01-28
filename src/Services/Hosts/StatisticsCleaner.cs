using Snap.Genshin.Website.Entities;

namespace Snap.Genshin.Website.Services.Hosts
{
    public class StatisticsCleaner : BackgroundService
    {
        public StatisticsCleaner(ILogger<StatisticsCleaner> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // 北京时间4:59
                if (DateTime.UtcNow.Minute != 59 || DateTime.UtcNow.Hour != 20)
                {
                    // 等待一分钟
                    await Task.Delay(60 * 1000, stoppingToken);
                    continue;
                }
                var localTime = DateTime.UtcNow + new TimeSpan(8, 0, 0);
                // 只有1日和16日能够触发
                if (localTime.Day != 1 && localTime.Day != 16) continue;

                // TODO 旧记录存档为文件

                // 删除所有旧记录
                var dbContext = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();
                dbContext.PlayerRecords.RemoveRange(dbContext.PlayerRecords);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            this.logger.LogInformation("数据库清理服务已启动...");
            return Task.CompletedTask;
        }
    }
}
