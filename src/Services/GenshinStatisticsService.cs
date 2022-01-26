using Snap.Genshin.Website.Configurations;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Services.StatisticCalculation;
using System.Reflection;

namespace Snap.Genshin.Website.Services
{
    public class GenshinStatisticsService
    {
        public GenshinStatisticsService(GenshinStatisticsServiceConfiguration options,
            ILogger<GenshinStatisticsService> logger,
            ApplicationDbContext dbContext,
            IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.calculatorTypes = options.CalculatorTypes;
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

            foreach (IStatisticCalculator? calculator in calculators)
            {
                logger.LogInformation("正在计算: {type}", calculator.GetType().Name);
                await calculator.Calculate();
            }
            logger.LogInformation("统计数据计算完毕。");
        }
    }
}
