using Microsoft.EntityFrameworkCore;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Entities.Record;
using Snap.HutaoAPI.Models.Statistics;
using Snap.HutaoAPI.Services.Abstraction;

namespace Snap.HutaoAPI.Services.StatisticCalculation
{
    [Obsolete("Should not use StatisticCalculation anymore")]
    public class AvatarParticipationCalculator : IStatisticCalculator
    {
        public AvatarParticipationCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        {
            this.dbContext = dbContext;
            this.statisticsProvider = statisticsProvider;
        }

        private readonly ApplicationDbContext dbContext;
        private readonly IStatisticsProvider statisticsProvider;

        public async Task Calculate()
        {
            // 忽略九层以下数据，忽略非满星数据
            IEnumerable<IGrouping<int, SpiralAbyssAvatar>>? floorGroup = dbContext.SpiralAbyssAvatars
                .Where(avatar => avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex >= 9)
                .Where(avatar => avatar.SpiralAbyssBattle.AbyssLevel.Star == 3)
                .Include(avatar => avatar.SpiralAbyssBattle)
                .ThenInclude(battle => battle.AbyssLevel)
                .AsEnumerable()
                .GroupBy(avatar => avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex);

            List<AvatarParticipation>? result = new();

            // 处理每层数据
            foreach (IGrouping<int, SpiralAbyssAvatar>? floor in floorGroup)
            {
                IEnumerable<IGrouping<int, SpiralAbyssAvatar>>? avatarGroups = floor
                    .AsEnumerable()
                    .GroupBy(avatar => avatar.AvatarId);

                IEnumerable<Rate<int>>? currentFloorData = avatarGroups
                    .Select(avararGroup => new Rate<int>
                    {
                        Id = avararGroup.Key,
                        Value = (double)avararGroup.Count() / floor.Count(),
                    });

                result.Add(new()
                {
                    Floor = floor.Key,
                    AvatarUsage = currentFloorData,
                });
            }

            await statisticsProvider.SaveStatistics<AvatarParticipationCalculator>(result);
        }
    }
}
