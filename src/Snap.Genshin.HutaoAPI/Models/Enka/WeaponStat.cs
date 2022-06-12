// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json.Serialization;

namespace Snap.HutaoAPI.Models.Enka;

/// <summary>
/// 武器属性
/// </summary>
public class WeaponStat
{
    /// <summary>
    /// 属性Id
    /// </summary>
    [JsonPropertyName("appendPropId")]
    public string AppendPropId { get; set; } = default!;

    /// <summary>
    /// 值
    /// </summary>
    [JsonPropertyName("statValue")]
    public double StatValue { get; set; }
}