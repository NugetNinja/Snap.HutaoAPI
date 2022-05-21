// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models.Statistics;

/// <summary>
/// 出场数据
/// </summary>
public class AvatarParticipation
{
    /// <summary>
    /// 层
    /// </summary>
    public int Floor { get; set; }

    /// <summary>
    /// 角色使用率
    /// </summary>
    public IEnumerable<Rate<int>> AvatarUsage { get; set; } = null!;
}
