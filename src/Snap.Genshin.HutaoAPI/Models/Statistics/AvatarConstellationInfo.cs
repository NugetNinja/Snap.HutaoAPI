// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models.Statistics;

/// <summary>
/// 角色命座信息
/// </summary>
public class AvatarConstellationInfo
{
    /// <summary>
    /// 角色Id
    /// </summary>
    public int Avatar { get; set; }

    /// <summary>
    /// 持有率
    /// </summary>
    public double HoldingRate { get; set; }

    /// <summary>
    /// 命座持有率
    /// </summary>
    public IEnumerable<Rate<int>> Rate { get; set; } = null!;
}
