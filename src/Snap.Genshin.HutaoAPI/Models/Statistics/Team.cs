// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models.Statistics;

public record Team
{
    /// <summary>
    /// 构造一个新的队伍
    /// </summary>
    /// <param name="upHalf">上半</param>
    /// <param name="downHalf">下半</param>
    public Team(string? upHalf, string? downHalf)
    {
        UpHalf = upHalf;
        DownHalf = downHalf;
    }

    /// <summary>
    /// 上半
    /// </summary>
    public string? UpHalf { get; set; }

    /// <summary>
    /// 下半
    /// </summary>
    public string? DownHalf { get; set; }

    /// <summary>
    /// 验证该队伍是否有效
    /// </summary>
    /// <returns>该队伍是否有效</returns>
    public bool Validate()
    {
        return (!string.IsNullOrEmpty(UpHalf)) && (!string.IsNullOrEmpty(DownHalf));
    }
}
