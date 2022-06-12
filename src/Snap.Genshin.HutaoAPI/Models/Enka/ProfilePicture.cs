// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json.Serialization;

namespace Snap.HutaoAPI.Models.Enka;

/// <summary>
/// 档案头像
/// </summary>
public class ProfilePicture
{
    /// <summary>
    /// 使用的角色Id
    /// </summary>
    [JsonPropertyName("avatarId")]
    public int AvatarId { get; set; }
}
