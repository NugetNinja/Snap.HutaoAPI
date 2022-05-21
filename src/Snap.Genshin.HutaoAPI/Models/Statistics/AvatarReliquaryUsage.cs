// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models.Statistics;

/// <summary>
/// 圣遗物配置数据
/// </summary>
public class AvatarReliquaryUsage
{
    /// <summary>
    /// 角色Id
    /// </summary>
    public int Avatar { get; set; }

    /// <summary>
    /// 角色圣遗物使用率
    /// </summary>
    public IEnumerable<Rate<string>> ReliquaryUsage { get; set; } = null!;
}
