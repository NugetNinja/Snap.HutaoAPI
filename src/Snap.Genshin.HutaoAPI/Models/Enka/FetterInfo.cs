// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json.Serialization;

namespace Snap.HutaoAPI.Models.Enka;

/// <summary>
/// 好感度信息
/// </summary>
public class FetterInfo
{
    /// <summary>
    /// 好感度等级
    /// </summary>
    [JsonPropertyName("expLevel")]
    public int ExpLevel { get; set; }
}
