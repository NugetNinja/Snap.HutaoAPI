// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.HutaoAPI.Models.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities;

/// <summary>
/// API调用者
/// </summary>
public class User : IUser
{
    /// <summary>
    /// UUID
    /// </summary>
    [NotMapped]
    public Guid UniqueUserId
    {
        get => AppId;
    }

    /// <summary>
    /// AppId
    /// </summary>
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public Guid AppId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Required]
    public string Name { get; set; } = null!;

    /// <inheritdoc/>
    public IDictionary<string, string> GetUserInfo()
    {
        return new Dictionary<string, string>
        {
            ["name"] = Name,
        };
    }
}
