// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.Genshin.Website.Services
{
    /// <summary>
    /// 统计计算器
    /// </summary>
    public interface IStatisticCalculator
    {
        /// <summary>
        /// 计算
        /// </summary>
        /// <returns>任务</returns>
        public Task Calculate();

        /// <summary>
        /// 获取深渊期数
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>深渊期数</returns>
        public static int GetSpiralPeriodId(DateTime time)
        {
            int periodNum = ((time.Year - 2000) * 12 + time.Month) * 2;

            // 上半月
            if (time.Day < 16 || time.Day == 16 && (time - time.Date).TotalMinutes < 240)
            {
                periodNum--;
            }

            // 上个月
            if (time.Day == 1 && (time - time.Date).TotalMinutes < 240)
            {
                periodNum--;
            }

            return periodNum;
        }
    }
}
