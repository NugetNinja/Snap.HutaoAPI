// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Services.Abstraction
{
    /// <summary>
    /// 统计管线
    /// </summary>
    public interface IStatisticPipeline
    {
        /// <summary>
        /// 计算并保存
        /// </summary>
        /// <returns>任务</returns>
        public Task CalculateAndSaveAsync();
    }
}
