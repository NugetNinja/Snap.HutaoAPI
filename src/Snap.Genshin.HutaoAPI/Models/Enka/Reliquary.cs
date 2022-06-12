// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json.Serialization;

namespace Snap.HutaoAPI.Models.Enka;
public class Reliquary
{
    /// <summary>
    /// 等级 +20 = 21
    /// [1,21]
    /// Artifact Level [1-21]
    /// </summary>
    [JsonPropertyName("level")]
    public int Level { get; set; }

    /// <summary>
    /// 主属性Id
    /// Artifact Main Stat ID
    /// </summary>
    [JsonPropertyName("mainPropId")]
    public int MainPropId { get; set; }

    /// <summary>
    /// 强化属性Id
    /// </summary>
    [JsonPropertyName("appendPropIdList")]
    public IList<int> AppendPropIdList { get; set; } = default!;
}
