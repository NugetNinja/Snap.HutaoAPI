using Microsoft.EntityFrameworkCore;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Entities.Record;
using Snap.HutaoAPI.Models.Statistics;

namespace Snap.HutaoAPI.Services.StatisticCalculation
{
    [Obsolete("Should not use StatisticCalculation anymore")]
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
            Dictionary<int, IDictionary<int, int>> avatarBattleWithWhoCountDic = new(128);
            Dictionary<int, int> avatarBattleWithAnyCountDic = new(128);
            foreach (DetailedBattleInfo battle in dbContext.SpiralAbyssBattles.Include(battle => battle.Avatars))
            {
                foreach (SpiralAbyssAvatar avatar in battle.Avatars)
                {
                    if (!avatarBattleWithWhoCountDic.ContainsKey(avatar.AvatarId))
                    {
                        avatarBattleWithWhoCountDic.Add(avatar.AvatarId, new Dictionary<int, int>(128));
                        avatarBattleWithAnyCountDic.Add(avatar.AvatarId, 0);
                    }
                    foreach (SpiralAbyssAvatar otherAvatar in battle.Avatars)
                    {
                        if (avatar.AvatarId == otherAvatar.AvatarId)
                        {
                            continue;
                        }

                        if (!avatarBattleWithWhoCountDic[avatar.AvatarId].ContainsKey(otherAvatar.AvatarId))
                        {
                            avatarBattleWithWhoCountDic[avatar.AvatarId].Add(otherAvatar.AvatarId, 0);
                        }

                        avatarBattleWithWhoCountDic[avatar.AvatarId][otherAvatar.AvatarId]++;
                        avatarBattleWithAnyCountDic[avatar.AvatarId]++;
                    }
                }
            }
            // 按照出现次数排序
            Dictionary<int, IEnumerable<(int AvatarId, int Count)>> avatarBattleWithWhoList = new(128);
            foreach (KeyValuePair<int, IDictionary<int, int>> pair in avatarBattleWithWhoCountDic)
            {
                avatarBattleWithWhoList.Add(
                    pair.Key,
                    from kv in pair.Value orderby kv.Value descending select (kv.Key, kv.Value));
            }

            List<TeamCollocation> result = new(128);
            foreach (KeyValuePair<int, IEnumerable<(int AvatarId, int Count)>> pair in avatarBattleWithWhoList)
            {
                result.Add(new()
                {
                    Avatar = pair.Key,
                    Collocations = (from count in pair.Value
                     select new Rate<int>()
                     {
                         Id = count.AvatarId,
                         Value = (double)count.Count / avatarBattleWithAnyCountDic[pair.Key],
                     }).Take(8),
                });
            }

            await statisticsProvider.SaveStatistics<TeamCollocationCalculator>(result);
        }
    }
}
