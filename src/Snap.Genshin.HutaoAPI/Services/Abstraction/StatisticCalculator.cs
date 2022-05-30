// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Services.Abstraction
{
    /// <summary>
    /// 统计计算器
    /// </summary>
    /// <typeparam name="TResult">计算结果的类型</typeparam>
    public abstract class StatisticCalculator<TResult> : IStatisticPipeline
        where TResult : notnull
    {
        private readonly IStatisticsProvider statisticsProvider;

        /// <summary>
        /// 构造一个新的统计计算器
        /// </summary>
        /// <param name="statisticsProvider">统计提供器</param>
        public StatisticCalculator(IStatisticsProvider statisticsProvider)
        {
            this.statisticsProvider = statisticsProvider;
        }

        /// <summary>
        /// 计算
        /// </summary>
        /// <returns>任务</returns>
        public abstract TResult Calculate();

        /// <inheritdoc/>
        public async Task CalculateAndSaveAsync()
        {
            TResult result = Calculate();
            await statisticsProvider.SaveStatisticAsync(GetType(), result);
        }
    }
}
