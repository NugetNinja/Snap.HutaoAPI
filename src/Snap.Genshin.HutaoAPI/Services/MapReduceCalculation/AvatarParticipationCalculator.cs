// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.HutaoAPI.Entities;

namespace Snap.HutaoAPI.Services.MapReduceCalculation
{
    /// <summary>
    /// use map reduce
    /// </summary>
    public class AvatarParticipationCalculator : IStatisticCalculator
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IStatisticsProvider statisticsProvider;

        /// <summary>
        /// 构造一个新的角色参与计算器
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="statisticsProvider">统计提供器</param>
        public AvatarParticipationCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        {
            this.dbContext = dbContext;
            this.statisticsProvider = statisticsProvider;
        }

        //TODO: mapreduce的方式待使用
        public Task Calculate()
        {
            return Task.CompletedTask;
        }
    }
}
