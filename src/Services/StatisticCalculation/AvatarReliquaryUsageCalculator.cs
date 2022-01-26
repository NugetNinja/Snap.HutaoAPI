using Snap.Genshin.Website.Entities;

namespace Snap.Genshin.Website.Services.StatisticCalculation
{
    public class AvatarReliquaryUsageCalculator : IStatisticCalculator
    {
        public AvatarReliquaryUsageCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        {
            this.dbContext = dbContext;
            this.statisticsProvider = statisticsProvider;
        }

        private readonly ApplicationDbContext dbContext;
        private readonly IStatisticsProvider statisticsProvider;

        public Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}
