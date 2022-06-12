// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json.Serialization;

namespace Snap.HutaoAPI.Models.Enka;

/// <summary>
/// 圣遗物主属性
/// </summary>
public class ReliquaryMainstat
{
    /// <summary>
    /// Equipment Append Property Name.
    /// </summary>
    [JsonPropertyName("mainPropId")]
    public string MainPropId { get; set; } = default!;

    /// <summary>
    /// Property Value
    /// </summary>
    [JsonPropertyName("statValue")]
    public double StatValue { get; set; }
}
