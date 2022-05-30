// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Services.Abstraction
{
    /// <summary>
    /// Statistics provider
    /// </summary>
    public interface IStatisticsProvider
    {
        /// <summary>
        /// 异步保存统计数据
        /// </summary>
        /// <param name="calculatorType">计算器类型</param>
        /// <param name="dataObject">数据</param>
        /// <returns>任务</returns>
        public Task SaveStatisticAsync(Type calculatorType, object dataObject);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="TCalculator">T</typeparam>
        /// <typeparam name="TResult">数据类型</typeparam>
        /// <returns>Task result</returns>
        public Task<TResult?> ReadStatisticAsync<TCalculator, TResult>()
            where TCalculator : StatisticCalculator<TResult>
            where TResult : notnull;
    }
}
