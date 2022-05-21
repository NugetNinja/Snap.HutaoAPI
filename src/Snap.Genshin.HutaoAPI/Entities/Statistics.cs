// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities;

/// <summary>
/// 统计
/// </summary>
public class Statistics
{
    /// <summary>
    /// id
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public long InnerId { get; set; }

    /// <summary>
    /// 源
    /// </summary>
    [Required]
    public string Source { get; set; } = null!;

    /// <summary>
    /// 值
    /// </summary>
    [Required]
    public string Value { get; set; } = null!;

    /// <summary>
    /// 深渊期数
    /// </summary>
    [Required]
    public int Period { get; set; }
}
