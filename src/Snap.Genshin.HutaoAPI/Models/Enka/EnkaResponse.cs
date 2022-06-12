// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json.Serialization;

namespace Snap.HutaoAPI.Models.Enka;

/// <summary>
/// Enka API 响应
/// </summary>
public class EnkaResponse
{
    /// <summary>
    /// 玩家基础信息
    /// </summary>
    [JsonPropertyName("playerInfo")]
    public PlayerInfo PlayerInfo { get; set; } = default!;

    /// <summary>
    /// 展示的角色详细信息列表
    /// </summary>
    [JsonPropertyName("avatarInfoList")]
    public IList<AvatarInfo> AvatarInfoList { get; set; } = default!;

    /// <summary>
    /// 刷新剩余秒数
    /// 生存时间值
    /// </summary>
    [JsonPropertyName("ttl")]
    public int Ttl { get; set; }
}