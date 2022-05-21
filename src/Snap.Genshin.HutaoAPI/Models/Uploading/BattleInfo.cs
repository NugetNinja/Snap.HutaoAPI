// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.HutaoAPI.Entities.Record;

namespace Snap.HutaoAPI.Models.Uploading
{
    /// <summary>
    /// 上下半间
    /// </summary>
    public class BattleInfo
    {
        /// <summary>
        /// 0 上半
        /// 1 下半
        /// </summary>
        public int BattleIndex { get; set; }

        /// <summary>
        /// 出场角色Id列表
        /// </summary>
        public List<int> AvatarIds { get; set; } = null!;

        /// <summary>
        /// 转换为详细上下半间信息
        /// </summary>
        /// <returns>详细上下半间信息</returns>
        public DetailedBattleInfo Complexify()
        {
            return new DetailedBattleInfo
            {
                Avatars = AvatarIds
                        .Select(avatar => new SpiralAbyssAvatar { AvatarId = avatar })
                        .ToList(),
                BattleIndex = BattleIndex,
            };
        }
    }
}