﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json.Serialization;

namespace Snap.HutaoAPI.Models.Enka;

/// <summary>
/// 武器
/// </summary>
public class Weapon
{
    /// <summary>
    /// 等级
    /// Weapon Level
    /// </summary>
    [JsonPropertyName("level")]
    public int Level { get; set; }

    /// <summary>
    /// 突破等级
    /// Weapon Ascension Level
    /// </summary>
    [JsonPropertyName("promoteLevel")]
    public int PromoteLevel { get; set; }

    /// <summary>
    /// 精炼 相较于实际等级 -1
    /// Weapon Refinement Level [0-4]
    /// </summary>
    [JsonPropertyName("affixMap")]
    public IDictionary<string, int> AffixMap { get; set; } = default!;
}
