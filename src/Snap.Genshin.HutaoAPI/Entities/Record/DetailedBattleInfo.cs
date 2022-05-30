// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities.Record;

/// <summary>
/// 深渊对战信息
/// </summary>
public class DetailedBattleInfo
{
    /// <summary>
    /// 对战id from db
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long InnerId { get; set; }

    /// <summary>
    /// 第几次战斗
    /// </summary>
    [Required]
    public int BattleIndex { get; set; }

    /// <summary>
    /// 上场角色的Id
    /// </summary>
    public IList<SpiralAbyssAvatar> Avatars { get; set; } = null!;

    /// <summary>
    /// 深渊层数
    /// </summary>
    [ForeignKey(nameof(SpiralAbyssLevelId))]
    public DetailedLevelInfo AbyssLevel { get; set; } = null!;

    /// <summary>
    /// 深渊层数外键
    /// </summary>
    public long SpiralAbyssLevelId { get; set; }
}
