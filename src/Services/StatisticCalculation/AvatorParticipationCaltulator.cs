using Snap.Genshin.Website.Entities;
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
            var count = dbContext.SpiralAbyssAvatars.Count();

            // 忽略十层以下数据
            var floorGroup = dbContext.SpiralAbyssAvatars
                .Where(avatar => avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex >= 10)
                .GroupBy(avatar => avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex);

            var result = new List<AvatarParticipation>();

            // 处理每层数据
            foreach (var floor in floorGroup)
            {
                var avatarGroups = floor.AsEnumerable().GroupBy(avatar => avatar.AvatarId);
                var currentFloorData = avatarGroups.Select(avararGroup => new Rate<int>
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
            var periodId = IStatisticCalculator.GetSpiralPeriodId(DateTime.Now);
            var data = dbContext.Statistics.Where(s => s.Source == nameof(AvatorParticipationCaltulator))
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
