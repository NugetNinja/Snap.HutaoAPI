// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities.Record;

/// <summary>
/// 深渊角色信息
/// 连表 将角色和深渊对战信息连在一起
/// </summary>
/// TODO 这个表真的有必要吗
public class SpiralAbyssAvatar
{
    /// <summary>
    /// inner id from db
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long InnerId { get; set; }

    /// <summary>
    /// 角色id
    /// </summary>
    [Required]
    public int AvatarId { get; set; }

    /// <summary>
    /// 外键 深渊对战信息
    /// </summary>
    [ForeignKey(nameof(SpiralAbyssBattleId))]
    public DetailedBattleInfo SpiralAbyssBattle { get; set; } = null!;

    /// <summary>
    /// 外键
    /// </summary>
    public long SpiralAbyssBattleId { get; set; }
}
