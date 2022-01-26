using Microsoft.EntityFrameworkCore;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Services.StatisticCalculation;
using System.Text.Json;

namespace Snap.Genshin.Website.Services
{
    public class StatisticsProvider : IStatisticsProvider
    {
        public StatisticsProvider(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private readonly ApplicationDbContext dbContext;

        public async Task SaveStatistics<TSource>(object dataObject)
        {
            var source = typeof(TSource).Name;

            // 新增或修改当期数据
            var periodId = IStatisticCalculator.GetSpiralPeriodId(DateTime.UtcNow);
            var data = dbContext.Statistics.Where(s => s.Source == source)
                                           .Where(s => s.Period == periodId)
                                           .SingleOrDefault();
            if (data is null)
            {
                data = new();
                dbContext.Statistics.Add(data);
            }
            data.Period = periodId;
            data.Source = source;
            data.Value = JsonSerializer.Serialize(dataObject);

            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<string?> ReadStatistics<TSource>()
        {
            var source = typeof(TSource).Name;

            // 查询当期数据
            var periodId = IStatisticCalculator.GetSpiralPeriodId(DateTime.UtcNow);
            var data = await dbContext.Statistics.Where(s => s.Source == source)
                                                 .Where(s => s.Period == periodId)
                                                 .SingleOrDefaultAsync();
            if (data is null) return null;
            return data.Value;
        }
    }
}
