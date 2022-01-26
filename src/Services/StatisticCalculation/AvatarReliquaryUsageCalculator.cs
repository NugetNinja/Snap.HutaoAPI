using Snap.Genshin.Website.Entities;

namespace Snap.Genshin.Website.Services.StatisticCalculation
{
    public class AvatarReliquaryUsageCalculator : IStatisticCalculator
    {
        public AvatarReliquaryUsageCalculator(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private readonly ApplicationDbContext dbContext;

        public Task Calculate()
        {
            throw new NotImplementedException();
        }
    }
}
