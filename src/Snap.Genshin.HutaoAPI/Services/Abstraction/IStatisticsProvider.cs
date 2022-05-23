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
        /// 保存统计数据
        /// </summary>
        /// <typeparam name="TCalculator">T</typeparam>
        /// <param name="dataObject">dataObject</param>
        /// <returns>Task</returns>
        public Task SaveStatistics<TCalculator>(object dataObject);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="TCalculator">T</typeparam>
        /// <returns>Task result</returns>
        public Task<string?> ReadStatistics<TCalculator>();
    }
}
