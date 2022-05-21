// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities;

/// <summary>
/// 用户Cliam
/// </summary>
public class UserClaim
{
    /// <summary>
    /// Id
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [Required]
    public string ClaimType { get; set; } = null!;

    /// <summary>
    /// 值
    /// </summary>
    [Required]
    public string ClaimValue { get; set; } = null!;

    /// <summary>
    /// 用户
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public User User { get; set; } = null!;

    /// <summary>
    /// 用户Id
    /// </summary>
    public Guid UserId { get; set; }
}
