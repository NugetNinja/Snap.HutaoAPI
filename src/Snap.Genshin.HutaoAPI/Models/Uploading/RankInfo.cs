// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;

namespace Snap.HutaoAPI.Models.Uploading
{

    /// <summary>
    /// 排行信息
    /// </summary>
    public class RankInfo
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        public int AvatarId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public int Value { get; set; }
    }
}