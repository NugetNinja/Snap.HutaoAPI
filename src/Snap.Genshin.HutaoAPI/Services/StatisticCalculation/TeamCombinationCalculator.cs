using Microsoft.EntityFrameworkCore;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Entities.Record;
using Snap.HutaoAPI.Models.Statistics;

namespace Snap.HutaoAPI.Services.StatisticCalculation
{
    [Obsolete("Should not use StatisticCalculation anymore")]
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
            List<DetailedBattleInfo> battles = dbContext.SpiralAbyssBattles
                .Where(battle => battle.AbyssLevel.FloorIndex >= 9)
                .Include(battle => battle.AbyssLevel)
                .ToList();
            IEnumerable<IGrouping<string, DetailedBattleInfo>> groups = battles
                .GroupBy(battle => $"{battle.AbyssLevel.FloorIndex}-{battle.AbyssLevel.LevelIndex}");

            List<LevelTeamUsage> result = new(12);

            foreach (IGrouping<string, DetailedBattleInfo>? group in groups)
            {
                IEnumerable<IGrouping<long, DetailedBattleInfo>> challenges = group
                    .GroupBy(battle => battle.SpiralAbyssLevelId);
                Dictionary<Team, int> countDic = new();
                foreach (IGrouping<long, DetailedBattleInfo>? challenge in challenges)
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

                var sorted = (from kv in countDic orderby kv.Value descending select kv).Take(24);

                string[] floor_index = group.Key.Split('-');
                //Convert.ToInt32 性能问题
                int floor = Convert.ToInt32(floor_index[0]);
                int index = Convert.ToInt32(floor_index[1]);

                result.Add(new(new FloorIndex(floor, index),
                    sorted.Select(kv => new Rate<Team>
                    {
                        Id = kv.Key,
                        Value = kv.Value
                    })));
            }

            await statisticsProvider.SaveStatistics<TeamCombinationCalculator>(result);
        }
    }
}
