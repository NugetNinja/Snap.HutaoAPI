// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.HutaoAPI.Entities.Record;
using System.Text.Json.Serialization;

namespace Snap.HutaoAPI.Models.Statistics;

public record Team
{
    /// <summary>
    /// 构造一个新的队伍
    /// </summary>
    /// <param name="upHalfAvatars">上半</param>
    /// <param name="downHalfAvatars">下半</param>
    public Team(IList<SpiralAbyssAvatar>? upHalfAvatars, IList<SpiralAbyssAvatar>? downHalfAvatars)
    {
        string upHalfAvatarsString = upHalfAvatars is null
                            ? string.Empty
                            : string.Join(',', upHalfAvatars
                                .OrderBy(avatar => avatar.AvatarId)
                                .Select(a => a.AvatarId));

        string downHalfAvatarsString = downHalfAvatars is null
            ? string.Empty
            : string.Join(',', downHalfAvatars
                .OrderBy(avatar => avatar.AvatarId)
                .Select(a => a.AvatarId));

        UpHalf = upHalfAvatarsString;
        DownHalf = downHalfAvatarsString;
    }

    /// <summary>
    /// 构造一个新的队伍
    /// </summary>
    /// <param name="upHalf">上半</param>
    /// <param name="downHalf">下半</param>
    [JsonConstructor]
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
