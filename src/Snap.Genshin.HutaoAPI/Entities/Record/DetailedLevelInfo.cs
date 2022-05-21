// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities.Record;

/// <summary>
/// 深渊关卡信息
/// </summary>
public class DetailedLevelInfo
{
    /// <summary>
    /// 深渊等级id from db
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long InnerId { get; set; }

    /// <summary>
    /// 外键
    /// </summary>
    [ForeignKey(nameof(RecordId))]
    public DetailedRecordInfo Record { get; set; } = null!;

    /// <summary>
    /// 外键 角色信息
    /// </summary>
    public long RecordId { get; set; }

    /// <summary>
    /// 层数
    /// </summary>
    [Required]
    public int FloorIndex { get; set; }

    /// <summary>
    /// 房间数
    /// </summary>
    [Required]
    public int LevelIndex { get; set; }

    /// <summary>
    /// 星数
    /// </summary>
    [Required]
    public int Star { get; set; }

    /// <summary>
    /// 战斗详情
    /// </summary>
    public IList<DetailedBattleInfo> Battles { get; set; } = null!;
}
