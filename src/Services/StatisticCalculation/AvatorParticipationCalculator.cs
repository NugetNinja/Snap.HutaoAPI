using Microsoft.EntityFrameworkCore;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Entities.Record;
using Snap.Genshin.Website.Models.Statistics;

namespace Snap.Genshin.Website.Services.StatisticCalculation
{
    public class AvatorParticipationCalculator : IStatisticCalculator
    {
        public AvatorParticipationCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        {
            this.dbContext = dbContext;
            this.statisticsProvider = statisticsProvider;
        }

        private readonly ApplicationDbContext dbContext;
        private readonly IStatisticsProvider statisticsProvider;

        public async Task Calculate()
        {
            // 忽略九层以下数据
            var floorGroup = dbContext.SpiralAbyssAvatars
                .Where(avatar => avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex >= 9)
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
                        Value = (double)avararGroup.Count() / floor.Count()
                    });

                result.Add(new()
                {
                    Floor = floor.Key,
                    AvatarUsage = currentFloorData,
                });
            }

            await statisticsProvider.SaveStatistics<AvatorParticipationCalculator>(result);
        }
    }
}
