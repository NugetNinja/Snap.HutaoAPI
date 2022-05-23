// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Models.Statistics;
using Snap.HutaoAPI.Services.Abstraction;

namespace Snap.HutaoAPI.Services.StatisticCalculation
{
    /// <summary>
    /// 总览数据计算器
    /// </summary>
    public class OverviewDataCalculator : IStatisticCalculator
    {
        /// <summary>
        /// 构造一个新的总览数据计算器
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="statisticsProvider">统计提供器</param>
        public OverviewDataCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        {
            this.dbContext = dbContext;
            this.statisticsProvider = statisticsProvider;
        }

        private readonly ApplicationDbContext dbContext;
        private readonly IStatisticsProvider statisticsProvider;

        /// <inheritdoc/>
        public async Task Calculate()
        {
            // 所有玩家数量
            int totalPlayerCount = dbContext.Players.Count();

            // 当期深渊数据量
            int collectedPlayerCount = dbContext.PlayerRecords.Count();

            // 满星玩家
            int fullStarPassedPlayerCount = dbContext.SpiralAbyssLevels
                .Where(record => record.FloorIndex == 12)
                .Where(record => record.Star == 3)
                .Select(record => record.Record.PlayerId)
                .Distinct()
                .Count();

            await statisticsProvider.SaveStatistics<OverviewDataCalculator>(new OverviewData
            {
                CollectedPlayerCount = collectedPlayerCount,
                TotalPlayerCount = totalPlayerCount,
                FullStarPlayerCount = fullStarPassedPlayerCount,
            });
        }
    }
}
