// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models.SnapGenshin
{
    public class PlayerAvatar
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
        public AvatarWeapon Weapon { get; set; } = null!;

        /// <summary>
        /// 圣遗物配置
        /// </summary>
        public List<AvatarReliquarySet> ReliquarySets { get; set; } = null!;
    }
}