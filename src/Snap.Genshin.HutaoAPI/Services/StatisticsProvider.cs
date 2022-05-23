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

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <typeparam name="TSource">T</typeparam>
        /// <param name="dataObject">dataObject</param>
        /// <returns>Task</returns>
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

        /// <summary>
        /// 读取统计数据
        /// </summary>
        /// <typeparam name="TSource">T</typeparam>
        /// <returns>Task result</returns>
        // TODO: 以后可以根据泛型来对传出的数据进行格式化
        public async Task<string?> ReadStatistics<TSource>()
        {
            // 正在计算统计数据时拒绝请求
            if (cache.TryGetValue("_STATISTICS_BUSY", out _))
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
