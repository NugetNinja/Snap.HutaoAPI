// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json.Serialization;

namespace Snap.HutaoAPI.Models.Statistics;

/// <summary>
/// 圣遗物配置数据
/// </summary>
public class AvatarReliquaryUsage
{
    /// <summary>
    /// 构造一个新的圣遗物配置数据
    /// </summary>
    /// <param name="avatar">角色id</param>
    /// <param name="reliquaryUsage">圣遗物使用率</param>
    [JsonConstructor]
    public AvatarReliquaryUsage(int avatar, IEnumerable<Rate<string>> reliquaryUsage)
    {
        Avatar = avatar;
        ReliquaryUsage = reliquaryUsage;
    }

    /// <summary>
    /// 角色Id
    /// </summary>
    public int Avatar { get; set; }

    /// <summary>
    /// 角色圣遗物使用率
    /// </summary>
    public IEnumerable<Rate<string>> ReliquaryUsage { get; set; } = null!;
}
