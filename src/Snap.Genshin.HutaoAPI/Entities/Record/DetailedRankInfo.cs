// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities.Record;

/// <summary>
/// 详细排行信息
/// </summary>
public class DetailedRankInfo
{
    /// <summary>
    /// 主键
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid InnerId { get; set; }

    /// <summary>
    /// 外键
    /// </summary>
    [ForeignKey(nameof(PlayerId))]
    public Player Player { get; set; } = null!;

    /// <summary>
    /// 外键对应的玩家id (From Db)
    /// </summary>
    public Guid PlayerId { get; set; }

    /// <summary>
    /// 角色Id
    /// </summary>
    [Required]
    public int AvatarId { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    [Required]
    public int Value { get; set; }

    /// <summary>
    /// 榜单类型
    /// </summary>
    [Required]
    public RankType Type { get; set; }

    /// <summary>
    /// 索引
    /// </summary>
    [NotMapped]
    public int Index { get; set; }
}
