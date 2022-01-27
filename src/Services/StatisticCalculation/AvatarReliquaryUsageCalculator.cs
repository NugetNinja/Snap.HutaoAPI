using Microsoft.EntityFrameworkCore;
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
            var ansDic = new Dictionary<int, Dictionary<string, int>>(128);
            var countDic = new Dictionary<int, int>(128);
            foreach (var avatar in dbContext.AvatarDetails.Include(avatar => avatar.ReliquarySets))
            {
                var sets = avatar.ReliquarySets.Where(set => set.Count >= 4);
                if (!sets.Any()) sets = avatar.ReliquarySets.Where(set => set.Count >= 2);
                    
                if (!ansDic.ContainsKey(avatar.AvatarId))
                {
                    ansDic.Add(avatar.AvatarId, new Dictionary<string, int>(32));
                    countDic.Add(avatar.AvatarId, 0);
                }
                    
                foreach(var set in sets)
                {
                    if (!ansDic[avatar.AvatarId].ContainsKey(set.UnionId))
                        ansDic[avatar.AvatarId].Add(set.UnionId, 0);
                    ansDic[avatar.AvatarId][set.UnionId]++;
                    countDic[avatar.AvatarId]++;
                }
            }

            var result = from kv in ansDic
                         select new AvatarReliquaryUsage
                         {
                             Avatar = kv.Key,
                             ReliquaryUsage = kv.Value.Select(kvp => new Rate<Models.SnapGenshin.AvatarReliquarySet>
                             {
                                 Id = new Models.SnapGenshin.AvatarReliquarySet
                                 {
                                     Id = Convert.ToInt32(kvp.Key.Split('-')[0]),
                                     Count = Convert.ToInt32(kvp.Key.Split('-')[1])
                                 },
                                 Value = (double)kvp.Value / countDic[kv.Key]
                             })
                         };

            await statisticsProvider.SaveStatistics<AvatarReliquaryUsageCalculator>(result);
        }
    }
}
