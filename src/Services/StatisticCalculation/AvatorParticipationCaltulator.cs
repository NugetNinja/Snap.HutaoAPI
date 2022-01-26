using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Entities.Record;
using Snap.Genshin.Website.Models.Statistics;
using System.Text.Json;

namespace Snap.Genshin.Website.Services.StatisticCalculation
{
    public class AvatorParticipationCaltulator : IStatisticCalculator
    {
        public AvatorParticipationCaltulator(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private readonly ApplicationDbContext dbContext;

        public async Task Calculate()
        {
            int count = dbContext.SpiralAbyssAvatars.Count();

            // 忽略九层以下数据
            IQueryable<IGrouping<int, SpiralAbyssAvatar>>? floorGroup = dbContext.SpiralAbyssAvatars
                .Where(avatar => avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex >= 9)
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
                        Value = (double)avararGroup.Count() / count
                    });

                result.Add(new()
                {
                    Floor = floor.Key,
                    AvatarUsage = currentFloorData,
                });
            }

            // 新增或修改当期数据
            int periodId = IStatisticCalculator.GetSpiralPeriodId(DateTime.UtcNow);
            Statistics? data = dbContext.Statistics
                .Where(s => s.Source == nameof(AvatorParticipationCaltulator))
                .Where(s => s.Period == periodId)
                .SingleOrDefault();
            if (data is null)
            {
                data = new();
                dbContext.Statistics.Add(data);
            }
            data.Period = periodId;
            data.Source = nameof(AvatorParticipationCaltulator);
            data.Value = JsonSerializer.Serialize(result);

            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
