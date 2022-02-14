using Microsoft.EntityFrameworkCore;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Entities.Record;
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
            List<SpiralAbyssBattle> battles = dbContext.SpiralAbyssBattles
                .Where(battle => battle.AbyssLevel.FloorIndex >= 9)
                .Include(battle => battle.AbyssLevel)
                .ToList();
            IEnumerable<IGrouping<string, SpiralAbyssBattle>> groups = battles
                .GroupBy(battle => $"{battle.AbyssLevel.FloorIndex}-{battle.AbyssLevel.LevelIndex}");

            List<LevelTeamUsage> result = new(12);

            foreach (IGrouping<string, SpiralAbyssBattle>? group in groups)
            {
                IEnumerable<IGrouping<long, SpiralAbyssBattle>> challenges = group
                    .GroupBy(battle => battle.SpiralAbyssLevelId);
                Dictionary<Team, int> countDic = new();
                foreach (IGrouping<long, SpiralAbyssBattle>? challenge in challenges)
                {
                    // 1-上半 2-下半
                    IList<SpiralAbyssAvatar>? upHalfAvatars = challenge
                        .Where(battle => battle.BattleIndex == 1)
                        .Select(battle => battle.Avatars).SingleOrDefault();
                    IList<SpiralAbyssAvatar>? downHalfAvatars = challenge
                        .Where(battle => battle.BattleIndex == 2)
                        .Select(battle => battle.Avatars).SingleOrDefault();

                    if (upHalfAvatars is null || downHalfAvatars is null)
                    {
                        continue;
                    }

                    Team team = new(
                        string.Join(',', upHalfAvatars
                        .OrderBy(avatar => avatar.AvatarId)
                        .Select(a => a.AvatarId)),
                        string.Join(',', downHalfAvatars
                        .OrderBy(avatar => avatar.AvatarId)
                        .Select(a => a.AvatarId)));

                    if (!countDic.ContainsKey(team))
                    {
                        countDic.Add(team, 0);
                    }

                    countDic[team]++;
                }

                string[] floor_index = group.Key.Split('-');
                //Convert.ToInt32 性能问题
                int floor = Convert.ToInt32(floor_index[0]);
                int index = Convert.ToInt32(floor_index[1]);

                result.Add(new(new LevelInfo(floor, index),
                    countDic.Select(kv => new Rate<Team>
                    {
                        Id = kv.Key,
                        Value = kv.Value
                    })));
            }

            await statisticsProvider.SaveStatistics<TeamCombinationCalculator>(result);
        }
    }
}
