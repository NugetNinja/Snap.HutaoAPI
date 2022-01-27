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
                if (!sets.Any()) sets = avatar.ReliquarySets.Where(set => set.Count >= 2)
                                                            .OrderBy(set => set.Id);

                // 全是散件或没装圣遗物，不统计
                if (!sets.Any()) continue;

                // 标准化装备数量
                foreach (var set in sets)
                {
                    if (set.Count >= 4) set.Count = 4;
                    else if (set.Count >= 2) set.Count = 2;
                    // TODO 该逻辑可否移动到Controller
                    set.UnionId = $"{set.Id}-{set.Count}";
                }

                var setString = string.Join(';', sets.Select(set => set.UnionId));

                if (!ansDic.ContainsKey(avatar.AvatarId))
                {
                    ansDic.Add(avatar.AvatarId, new Dictionary<string, int>(32));
                    countDic.Add(avatar.AvatarId, 0);
                }

                if (!ansDic[avatar.AvatarId].ContainsKey(setString))
                    ansDic[avatar.AvatarId].Add(setString, 0);
                ansDic[avatar.AvatarId][setString]++;
                countDic[avatar.AvatarId]++;
            }

            var result = from kv in ansDic
                         select new AvatarReliquaryUsage
                         {
                             Avatar = kv.Key,
                             ReliquaryUsage = kv.Value.Select(kvp => new Rate<string>
                             {
                                 Id = kvp.Key,
                                 Value = (double)kvp.Value / countDic[kv.Key]
                             })
                         };

            await statisticsProvider.SaveStatistics<AvatarReliquaryUsageCalculator>(result);
        }
    }
}
