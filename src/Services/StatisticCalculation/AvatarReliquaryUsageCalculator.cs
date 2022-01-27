using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Models.Statistics;

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

        public async Task Calculate()
        {
            var ansDic = new Dictionary<int, Dictionary<string, int>>();
            foreach (var avatar in dbContext.AvatarDetails)
            {
                var sets = avatar.ReliquarySets.Where(set => set.Count >= 4);
                if (!sets.Any()) sets = avatar.ReliquarySets.Where(set => set.Count >= 2);
                    
                if (!ansDic.ContainsKey(avatar.AvatarId))
                    ansDic.Add(avatar.AvatarId, new Dictionary<string, int>());
                foreach(var set in sets)
                {
                    if (!ansDic[avatar.AvatarId].ContainsKey(set.UnionId))
                        ansDic[avatar.AvatarId].Add(set.UnionId, 0);
                    ansDic[avatar.AvatarId][set.UnionId]++;
                }
            }

            var result = from kv in ansDic
                select new AvatarReliquaryUsage
                {
                    Avatar = kv.Key,
                    ReliquaryUsage = kv.Value.Select(kv => new Rate<Models.SnapGenshin.AvatarReliquarySet>
                    {
                        Id = new Models.SnapGenshin.AvatarReliquarySet
                        {
                            Id = Convert.ToInt32(kv.Key.Split('-')[0]),
                            Count = Convert.ToInt32(kv.Key.Split('-')[1])
                        }
                    })
                };

            await statisticsProvider.SaveStatistics<AvatarReliquaryUsageCalculator>(result);
        }
    }
}
