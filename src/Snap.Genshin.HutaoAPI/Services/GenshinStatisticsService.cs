using Snap.HutaoAPI.Configurations;
using Snap.HutaoAPI.Entities;
using System.Diagnostics;

namespace Snap.HutaoAPI.Services
{
    public class GenshinStatisticsService
    {
        public GenshinStatisticsService(
            GenshinStatisticsServiceConfiguration options,
            ILogger<GenshinStatisticsService> logger,
            ApplicationDbContext dbContext,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            calculatorTypes = options.CalculatorTypes;
            this.serviceProvider = serviceProvider;
        }

        private readonly ILogger logger;
        private readonly ApplicationDbContext dbContext;
        private readonly List<Type> calculatorTypes;
        private readonly IServiceProvider serviceProvider;

        public async Task CaltulateStatistics()
        {
            logger.LogInformation("开始计算统计数据...");

            IEnumerable<IStatisticCalculator>? calculators =
                from type in calculatorTypes
                select serviceProvider.GetRequiredService(type) as IStatisticCalculator;

            Stopwatch? watch = new Stopwatch();

            foreach (IStatisticCalculator? calculator in calculators)
            {
                watch.Restart();
                await calculator.Calculate();
                watch.Stop();

                logger.LogInformation("计算: {type}完成，用时{time}ms。", calculator.GetType().Name, watch.Elapsed.TotalMilliseconds);
            }
            logger.LogInformation("统计数据计算完毕。");
        }
    }
}
