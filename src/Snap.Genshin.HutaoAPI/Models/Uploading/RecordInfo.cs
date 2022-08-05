// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Snap.HutaoAPI.Models.Uploading
{
    /// <summary>
    /// 上传的深渊记录
    /// </summary>
    public class RecordInfo
    {
        /// <summary>
        /// 玩家Uid
        /// </summary>
        [MaxLength(9)]
        public string Uid { get; set; } = null!;

        /// <summary>
        /// 角色信息
        /// </summary>
        public List<AvatarInfo> PlayerAvatars { get; set; } = null!;

        /// <summary>
        /// 深渊信息
        /// </summary>
        public List<LevelInfo> PlayerSpiralAbyssesLevels { get; set; } = null!;

        /// <summary>
        /// 造成最多伤害
        /// </summary>
        public RankInfo? DamageMost { get; set; }

        /// <summary>
        /// 承受最多伤害
        /// </summary>
        public RankInfo? TakeDamageMost { get; set; }

        /// <summary>
        /// 验证上传记录的有效性
        /// </summary>
        /// <returns>是否有效</returns>
        public bool Validate()
        {
            if (Uid == null || Uid.Length != 9)
            {
                return false;
            }

            if (PlayerAvatars == null || PlayerAvatars.Count <= 0)
            {
                return false;
            }

            return PlayerAvatars.Any(a => a.Id is 10000005 or 10000007);
        }
    }
}