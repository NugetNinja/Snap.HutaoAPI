// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.HutaoAPI.Entities.Record;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities;

/// <summary>
/// 玩家实体
/// </summary>
public class Player
{
    /// <summary>
    /// 为了EFC
    /// </summary>
    public Player()
    {
    }

    /// <summary>
    /// 构造一个新的玩家
    /// </summary>
    /// <param name="uid">uid</param>
    /// <param name="avatars">角色列表</param>
    public Player(string uid, List<DetailedAvatarInfo> avatars)
    {
        Uid = uid;
        Avatars = avatars;
    }

    /// <summary>
    /// 玩家Id (from database)
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid InnerId { get; set; }

    /// <summary>
    /// 玩家uid
    /// </summary>
    [Required]
    public string Uid { get; set; } = null!;

    /// <summary>
    /// 角色列表
    /// </summary>
    public IList<DetailedAvatarInfo> Avatars { get; set; } = null!;
}
