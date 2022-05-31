// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models.Statistics;

/// <summary>
/// 组队数据
/// </summary>
public class TeamCollocation
{
    /// <summary>
    /// 角色Id
    /// </summary>
    public int Avatar { get; set; }

    /// <summary>
    /// 角色配队率
    /// </summary>
    public IEnumerable<Rate<int>> Collocations { get; set; } = null!;
}
