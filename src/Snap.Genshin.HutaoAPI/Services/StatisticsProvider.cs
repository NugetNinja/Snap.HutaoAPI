// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Services.Abstraction;
using System.Text.Json;

namespace Snap.HutaoAPI.Services
{
    /// <summary>
    /// 统计数据实现
    /// </summary>
    public class StatisticsProvider : IStatisticsProvider
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContext">db from di</param>
        /// <param name="cache">cache from di</param>
        public StatisticsProvider(ApplicationDbContext dbContext, IMemoryCache cache)
        {
            this.dbContext = dbContext;
            this.cache = cache;
        }

        private readonly ApplicationDbContext dbContext;
        private readonly IMemoryCache cache;

        /// <inheritdoc/>
        public async Task SaveStatisticAsync(Type calculatorType, object dataObject)
        {
            int periodId = GetSpiralPeriodId(DateTime.Now);
            string source = calculatorType.Name;
            string dataKey = $"_STATISTICS_{source}_VALUE_";

            // 新增或修改当期数据
            Statistics? data = await dbContext.Statistics
                .Where(s => s.Source == source)
                .Where(s => s.Period == periodId)
                .SingleOrDefaultAsync();

            if (data is null)
            {
                data = new();
                dbContext.Statistics.Add(data);
            }

            data.Period = periodId;
            data.Source = source;

            // update it's value
            data.Value = JsonSerializer.Serialize(dataObject);
            cache.Set(dataKey, data);

            await dbContext.SaveChangesAsync()
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<TResult?> ReadStatisticAsync<TCalculator, TResult>()
            where TCalculator : StatisticCalculator<TResult>
            where TResult : notnull
        {
            // 正在计算统计数据时拒绝请求
            if (cache.TryGetValue("_STATISTICS_BUSY", out _))
            {
                return default;
            }

            string? source = typeof(TCalculator).Name;

            // 查询当期数据
            int periodId = GetSpiralPeriodId(DateTime.Now);

            string dataKey = $"_STATISTICS_{source}_VALUE_";

            // 获取缓存中数据
            if (!cache.TryGetValue(dataKey, out Statistics? data))
            {
                data = await dbContext.Statistics
                    .Where(s => s.Source == source)
                    .Where(s => s.Period == periodId)
                    .SingleOrDefaultAsync();
                cache.Set(dataKey, data);
            }

            return data is null
                ? default
                : JsonSerializer.Deserialize<TResult>(data.Value);
        }

        /// <summary>
        /// 获取深渊期数
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>深渊期数</returns>
        private int GetSpiralPeriodId(DateTime time)
        {
            int periodNum = (((time.Year - 2000) * 12) + time.Month) * 2;

            TimeSpan fourHours = TimeSpan.FromHours(4);

            // 上半月
            if (time.Day < 16 || (time.Day == 16 && ((time - time.Date) < fourHours)))
            {
                periodNum--;
            }

            // 上个月
            if (time.Day == 1 && ((time - time.Date) < fourHours))
            {
                periodNum--;
            }

            return periodNum;
        }
    }
}
