// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models.Statistics;

/// <summary>
/// 武器使用数据
/// </summary>
public class WeaponUsage
{
    /// <summary>
    /// 角色
    /// </summary>
    public int Avatar { get; set; }

    /// <summary>
    /// 武器
    /// </summary>
    public IEnumerable<Rate<int>> Weapons { get; set; } = null!;
}
