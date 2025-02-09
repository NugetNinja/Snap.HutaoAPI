﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities.Record;

/// <summary>
/// 圣遗物套装信息
/// </summary>
public class DetailedReliquarySetInfo
{
    /// <summary>
    /// 圣遗物id from db
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long InnerId { get; set; }

    /// <summary>
    /// 套装Id (from Mihoyo)
    /// </summary>
    [Required]
    public int Id { get; set; }

    /// <summary>
    /// 装备该套装的数量
    /// </summary>
    [Required]
    public int Count { get; set; }

    /// <summary>
    /// 格式：{Id}-{Count}
    /// </summary>
    [Obsolete("不应使用此字段")]
    public string UnionId { get; set; } = string.Empty;

    /// <summary>
    /// 外键
    /// </summary>
    [ForeignKey(nameof(AvatarDetailId))]
    public DetailedAvatarInfo AvatarInfo { get; set; } = null!;

    /// <summary>
    /// 外键 对应的角色id
    /// </summary>
    public long AvatarDetailId { get; set; }

    /// <summary>
    /// 格式：{Id}-{Count}
    /// </summary>
    /// <returns>联合id</returns>
    public string GetUnionId()
    {
        return $"{Id}-{Count}";
    }
}
