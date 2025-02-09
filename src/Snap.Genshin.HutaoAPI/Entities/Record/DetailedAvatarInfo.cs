﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities.Record;

/// <summary>
/// 角色详细信息实体
/// </summary>
public class DetailedAvatarInfo
{
    /// <summary>
    /// 角色详细信息实体Id (from database)
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long InnerId { get; set; }

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
    /// 角色等级
    /// </summary>
    [Required]
    public int AvatarLevel { get; set; }

    /// <summary>
    /// 已激活命座数
    /// </summary>
    [Required]
    public int ActivedConstellationNum { get; set; }

    /// <summary>
    /// 角色武器Id
    /// </summary>
    [Required]
    public int WeaponId { get; set; }

    /// <summary>
    /// 武器等级
    /// </summary>
    [Required]
    public int WeaponLevel { get; set; }

    /// <summary>
    /// 武器精炼等级
    /// </summary>
    [Required]
    public int AffixLevel { get; set; }

    /// <summary>
    /// 圣遗物配置信息
    /// </summary>
    public IList<DetailedReliquarySetInfo> ReliquarySets { get; set; } = null!;

    /// <summary>
    /// 获取标准化的圣遗物个数
    /// </summary>
    /// <returns>标准化的圣遗物个数</returns>
    public IList<DetailedReliquarySetInfo> GetNormalizedReliquarySets()
    {
        IEnumerable<DetailedReliquarySetInfo> sets = ReliquarySets
            .Where(set => set.Count >= 2);

        // 标准化装备数量
        foreach (DetailedReliquarySetInfo set in sets)
        {
            set.Count = set.Count >= 4 ? 4 : 2;
        }

        return sets.ToList();
    }
}
