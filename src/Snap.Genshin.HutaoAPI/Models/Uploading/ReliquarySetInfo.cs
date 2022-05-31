// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.HutaoAPI.Entities.Record;

namespace Snap.HutaoAPI.Models.Uploading
{
    /// <summary>
    /// 圣遗物套装
    /// </summary>
    public class ReliquarySetInfo
    {
        /// <summary>
        /// 套装Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 装备数量
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// 转化到详细圣遗物信息
        /// </summary>
        /// <returns>详细圣遗物信息</returns>
        public DetailedReliquarySetInfo Complexify()
        {
            return new DetailedReliquarySetInfo()
            {
                Id = Id,
                Count = Count,
            };
        }
    }
}