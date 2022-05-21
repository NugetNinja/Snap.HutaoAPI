// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Services
{
    /// <summary>
    /// Statistics provider
    /// </summary>
    public interface IStatisticsProvider
    {
        /// <summary>
        /// 保存统计数据
        /// </summary>
        /// <typeparam name="TSource">T</typeparam>
        /// <param name="dataObject">dataObject</param>
        /// <returns>Task</returns>
        public Task SaveStatistics<TSource>(object dataObject);

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <typeparam name="TSource">T</typeparam>
        /// <returns>Task result</returns>
        public Task<string?> ReadStatistics<TSource>();
    }
}
