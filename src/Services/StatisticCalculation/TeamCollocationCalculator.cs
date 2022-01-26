using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Models.Statistics;
using System.Linq;
using System.Text.Json;

namespace Snap.Genshin.Website.Services.StatisticCalculation
{
    public class TeamCollocationCalculator : IStatisticCalculator
    {
        public TeamCollocationCalculator(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private readonly ApplicationDbContext dbContext;

        public async Task Calculate()
        {
            var avatarBattleWithWhoCountDic = new Dictionary<int, IDictionary<int, int>>(128);
            var avatarBattleWithAnyCountDic = new Dictionary<int, int>(128);
            foreach (var battle in dbContext.SpiralAbyssBattles)
            {
                foreach(var avatar in battle.Avatars)
                {
                    if (!avatarBattleWithWhoCountDic.ContainsKey(avatar.AvatarId))
                    {
                        avatarBattleWithWhoCountDic.Add(avatar.AvatarId, new Dictionary<int, int>(128));
                        avatarBattleWithAnyCountDic.Add(avatar.AvatarId, 0);
                    }
                    foreach(var otherAvatar in battle.Avatars)
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
            foreach(var pair in avatarBattleWithWhoCountDic)
            {
                avatarBattleWithWhoList.Add(pair.Key, from kv in pair.Value orderby kv.Value descending select (kv.Key, kv.Value));
            }

            var result = new List<TeamCollocation>(128);
            foreach(var pair in avatarBattleWithWhoList)
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

            // 新增或修改当期数据
            var periodId = IStatisticCalculator.GetSpiralPeriodId(DateTime.Now);
            var data = dbContext.Statistics.Where(s => s.Source == nameof(TeamCollocationCalculator))
                                           .Where(s => s.Period == periodId)
                                           .SingleOrDefault();
            if (data is null)
            {
                data = new();
                dbContext.Statistics.Add(data);
            }
            data.Period = periodId;
            data.Source = nameof(TeamCollocationCalculator);
            data.Value = JsonSerializer.Serialize(result);

            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
