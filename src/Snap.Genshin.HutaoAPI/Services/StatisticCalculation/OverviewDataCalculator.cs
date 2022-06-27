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
    public class OverviewDataCalculator : StatisticCalculator<OverviewData>
    {
        private readonly ApplicationDbContext dbContext;

        /// <summary>
        /// 构造一个新的总览数据计算器
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="statisticsProvider">统计提供器</param>
        public OverviewDataCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
            : base(statisticsProvider)
        {
            this.dbContext = dbContext;
        }

        /// <inheritdoc/>
        public override OverviewData Calculate()
        {
            // 所有玩家数量
            int totalPlayerCount = dbContext.Players.Count();

            // 当期深渊数据量
            int collectedPlayerCount = dbContext.PlayerRecords.Count();

            // 满星玩家
            int fullStarPassedPlayerCount = dbContext.SpiralAbyssLevels
                .Where(level => level.FloorIndex == 12)
                .Where(level => level.Star == 3)
                .Select(record => record.Record.PlayerId)
                .Distinct()
                .Count();

            return new()
            {
                CollectedPlayerCount = collectedPlayerCount,
                TotalPlayerCount = totalPlayerCount,
                FullStarPlayerCount = fullStarPassedPlayerCount,
            };
        }
    }
}
