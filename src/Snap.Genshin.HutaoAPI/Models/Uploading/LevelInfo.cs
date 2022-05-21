// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.HutaoAPI.Entities.Record;

namespace Snap.HutaoAPI.Models.Uploading
{
    /// <summary>
    /// 层信息
    /// </summary>
    public class LevelInfo
    {
        /// <summary>
        /// 层数
        /// </summary>
        public int FloorIndex { get; set; }

        /// <summary>
        /// 第几间
        /// </summary>
        public int LevelIndex { get; set; }

        /// <summary>
        /// 星数
        /// </summary>
        public int Star { get; set; }

        /// <summary>
        /// 战斗详情
        /// </summary>
        public List<BattleInfo> Battles { get; set; } = null!;

        /// <summary>
        /// 转换到详细层信息
        /// </summary>
        /// <returns>详细层信息</returns>
        public DetailedLevelInfo Complexify()
        {
            return new DetailedLevelInfo
            {
                FloorIndex = FloorIndex,
                LevelIndex = LevelIndex,
                Star = Star,
                Battles = Battles.Select(battle => battle.Complexify()).ToList(),
            };
        }
    }
}