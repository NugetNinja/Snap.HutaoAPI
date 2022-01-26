using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Models.Statistics;

namespace Snap.Genshin.Website.Services.StatisticCalculation
{
    public class TeamCollocationCalculator : IStatisticCalculator
    {
        public TeamCollocationCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        {
            this.dbContext = dbContext;
            this.statisticsProvider = statisticsProvider;
        }

        private readonly ApplicationDbContext dbContext;
        private readonly IStatisticsProvider statisticsProvider;

        public async Task Calculate()
        {
            var avatarBattleWithWhoCountDic = new Dictionary<int, IDictionary<int, int>>(128);
            var avatarBattleWithAnyCountDic = new Dictionary<int, int>(128);
            foreach (var battle in dbContext.SpiralAbyssBattles)
            {
                foreach (var avatar in battle.Avatars)
                {
                    if (!avatarBattleWithWhoCountDic.ContainsKey(avatar.AvatarId))
                    {
                        avatarBattleWithWhoCountDic.Add(avatar.AvatarId, new Dictionary<int, int>(128));
                        avatarBattleWithAnyCountDic.Add(avatar.AvatarId, 0);
                    }
                    foreach (var otherAvatar in battle.Avatars)
                    {
                        if (avatar.AvatarId == otherAvatar.AvatarId) continue;
                        if (!avatarBattleWithWhoCountDic[avatar.AvatarId].ContainsKey(otherAvatar.AvatarId))
                            avatarBattleWithWhoCountDic[avatar.AvatarId].Add(otherAvatar.AvatarId, 0);
                        avatarBattleWithWhoCountDic[avatar.AvatarId][otherAvatar.AvatarId]++;
                        avatarBattleWithAnyCountDic[avatar.AvatarId]++;
                    }
                }
            }
            // 按照出现次数排序
            var avatarBattleWithWhoList = new Dictionary<int, IEnumerable<(int AvatarId, int Count)>>(128);
            foreach (var pair in avatarBattleWithWhoCountDic)
            {
                avatarBattleWithWhoList.Add(pair.Key, from kv in pair.Value orderby kv.Value descending select (kv.Key, kv.Value));
            }

            var result = new List<TeamCollocation>(128);
            foreach (var pair in avatarBattleWithWhoList)
            {
                result.Add(new()
                {
                    Avater = pair.Key,
                    Collocations = (from count in pair.Value
                                    select new Rate<int>()
                                    {
                                        Id = count.AvatarId,
                                        Value = (double)count.Count / avatarBattleWithAnyCountDic[pair.Key]
                                    }).Take(8)
                });
            }

            await statisticsProvider.SaveStatistics<TeamCollocationCalculator>(result);
        }
    }
}
