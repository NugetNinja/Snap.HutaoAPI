using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Entities.Record;
using Snap.HutaoAPI.Models.Statistics;

namespace Snap.HutaoAPI.Services.StatisticCalculation
{
    [Obsolete("Should not use StatisticCalculation anymore")]
    public class ActivedConstellationNumCalculator : IStatisticCalculator
    {
        public ActivedConstellationNumCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        {
            this.dbContext = dbContext;
            this.statisticsProvider = statisticsProvider;
        }

        private readonly ApplicationDbContext dbContext;
        private readonly IStatisticsProvider statisticsProvider;

        public async Task Calculate()
        {
            double totalPlayerCount = dbContext.Players.Count();

            IEnumerable<IGrouping<int, AvatarDetail>> avatarGroups = dbContext.AvatarDetails
                .AsEnumerable()
                .GroupBy(avatar => avatar.AvatarId);

            List<AvatarConstellationNum> result = new(128);

            foreach (IGrouping<int, AvatarDetail>? avatarGroup in avatarGroups)
            {
                Dictionary<int, int> countDic = Enumerable
                    .Range(0, 7)
                    .ToDictionary(key => key, value => 0);

                foreach (AvatarDetail? avatar in avatarGroup)
                {
                    countDic[avatar.ActivedConstellationNum]++;
                }

                IEnumerable<Rate<int>> rate =
                    from kv in countDic
                    select new Rate<int> { Id = kv.Key, Value = (double)kv.Value / avatarGroup.Count() };

                int avatarHolding = countDic.Sum(kv => kv.Value);
                double holdingRate = avatarHolding / totalPlayerCount;

                result.Add(new AvatarConstellationNum { Avatar = avatarGroup.Key, HoldingRate = holdingRate, Rate = rate });
            }

            await statisticsProvider.SaveStatistics<ActivedConstellationNumCalculator>(result);
        }
    }
}
