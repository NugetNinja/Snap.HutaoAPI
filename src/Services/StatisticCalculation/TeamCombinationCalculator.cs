using Microsoft.EntityFrameworkCore;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Models.Statistics;

namespace Snap.Genshin.Website.Services.StatisticCalculation
{
    public class TeamCombinationCalculator : IStatisticCalculator
    {
        public TeamCombinationCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        {
            this.dbContext = dbContext;
            this.statisticsProvider = statisticsProvider;
        }

        private readonly ApplicationDbContext dbContext;
        private readonly IStatisticsProvider statisticsProvider;

        public async Task Calculate()
        {
            var battles = dbContext.SpiralAbyssBattles.Where(battle => battle.AbyssLevel.FloorIndex >= 9).Include(battle => battle.AbyssLevel).ToList();
            var groups = battles.GroupBy(battle => $"{battle.AbyssLevel.FloorIndex}-{battle.AbyssLevel.LevelIndex}");

            var result = new List<LevelTeamUsage>(12);
            
            foreach (var group in groups)
            {
                var challenges = group.GroupBy(battle => battle.SpiralAbyssLevelId);
                var countDic = new Dictionary<Team, int>();
                foreach (var challenge in challenges)
                {
                    // 1-上半 2-下半
                    var upHalfAvatars = challenge.Where(battle => battle.BattleIndex == 1).Select(battle=>battle.Avatars).SingleOrDefault();
                    var downHalfAvatars = challenge.Where(battle => battle.BattleIndex == 2).Select(battle => battle.Avatars).SingleOrDefault();
                    if (upHalfAvatars is null || downHalfAvatars is null) continue;
                    var team = new Team(string.Join(',', upHalfAvatars.OrderBy(avatar => avatar.AvatarId).Select(a => a.AvatarId)),
                        string.Join(',', downHalfAvatars.OrderBy(avatar => avatar.AvatarId).Select(a => a.AvatarId)));

                    if (!countDic.ContainsKey(team)) countDic.Add(team, 0);
                    countDic[team]++;
                }

                var floor_index = group.Key.Split('-');
                var floor = Convert.ToInt32(floor_index[0]);
                var index = Convert.ToInt32(floor_index[1]);

                result.Add(new(new LevelInfo(floor, index), countDic.Select(kv => new Rate<Team>
                {
                    Id = kv.Key,
                    Value = kv.Value
                })));
            }

            await statisticsProvider.SaveStatistics<TeamCombinationCalculator>(result);
        }
    }
}
