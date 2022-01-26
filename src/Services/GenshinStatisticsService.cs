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
            ApplicationDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
            this.calculatorConstructors = options.CalculatorConstructors;
        }

        private readonly ILogger logger;
        private readonly ApplicationDbContext dbContext;
        private readonly IEnumerable<ConstructorInfo> calculatorConstructors;

        public async Task CaltulateStatistics()
        {
            logger.LogInformation("开始计算统计数据...");
            var calculators = from constructor in calculatorConstructors
                              let parameters = new object[] { dbContext }
                              select constructor.Invoke(parameters) as IStatisticCalculator;
            foreach (var calculator in calculators)
            {
                logger.LogInformation("正在计算: {type}", calculator.GetType().Name);
                await calculator.Calculate();
            }
            logger.LogInformation("统计数据计算完毕。");
        }
    }
}
