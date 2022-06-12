// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json.Serialization;

namespace Snap.HutaoAPI.Models.Enka;

/// <summary>
/// 类型与值
/// </summary>
public class TypeValue
{
    /// <summary>
    /// 类型Id
    /// </summary>
    [JsonPropertyName("type")]
    public int Type { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    [JsonPropertyName("val")]
    public string? Value { get; set; }
}
