using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Services.StatisticCalculation;
using System.Text.Json;

namespace Snap.Genshin.Website.Services
{
    public class StatisticsProvider : IStatisticsProvider
    {
        public StatisticsProvider(ApplicationDbContext dbContext, IMemoryCache cache)
        {
            this.dbContext = dbContext;
            this.cache = cache;
        }

        private readonly ApplicationDbContext dbContext;
        private readonly IMemoryCache cache;

        public async Task SaveStatistics<TSource>(object dataObject)
        {
            string? source = typeof(TSource).Name;

            // 新增或修改当期数据
            int periodId = IStatisticCalculator.GetSpiralPeriodId(DateTime.UtcNow);
            Statistics? data = dbContext.Statistics
                .Where(s => s.Source == source)
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
            // 正在计算统计数据时拒绝请求
            bool isBusy = cache.TryGetValue("_STATISTICS_BUSY", out _);
            if (isBusy)
            {
                return null;
            }

            string? source = typeof(TSource).Name;

            // 查询当期数据
            int periodId = IStatisticCalculator.GetSpiralPeriodId(DateTime.UtcNow);
            Statistics? data = await dbContext.Statistics
                .Where(s => s.Source == source)
                .Where(s => s.Period == periodId)
                .SingleOrDefaultAsync();

            return data?.Value;
        }
    }
}
