// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.HutaoAPI.Entities.Record;

namespace Snap.HutaoAPI.Models.Uploading
{
    /// <summary>
    /// 角色
    /// </summary>
    public class AvatarInfo
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 角色等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 命座数
        /// </summary>
        public int ActivedConstellationNum { get; set; }

        /// <summary>
        /// 武器信息
        /// </summary>
        public WeaponInfo Weapon { get; set; } = null!;

        /// <summary>
        /// 圣遗物配置
        /// </summary>
        public List<ReliquarySetInfo> ReliquarySets { get; set; } = null!;

        /// <summary>
        /// 转化为详细角色信息
        /// </summary>
        /// <returns>详细角色信息</returns>
        public DetailedAvatarInfo Complexify()
        {
            return new DetailedAvatarInfo()
            {
                AvatarId = Id,
                AvatarLevel = Level,
                WeaponId = Weapon.Id,
                WeaponLevel = Weapon.Level,
                AffixLevel = Weapon.AffixLevel,
                ActivedConstellationNum = ActivedConstellationNum,
                ReliquarySets = ReliquarySets.Select(r => r.Complexify()).ToList(),
            };
        }
    }
}