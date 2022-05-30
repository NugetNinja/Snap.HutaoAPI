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

    public static Team? FromBattleInfo(IEnumerable<DetailedBattleInfo> battleInfos)
    {
        IEnumerable<int>? up = battleInfos
            .Where(battle => battle.BattleIndex == 1)
            .SingleOrDefault()?
            .Avatars
            .OrderBy(avatar => avatar.AvatarId)
            .Select(a => a.AvatarId);

        IEnumerable<int>? down = battleInfos
            .Where(battle => battle.BattleIndex == 2)
            .SingleOrDefault()?
            .Avatars
            .OrderBy(avatar => avatar.AvatarId)
            .Select(a => a.AvatarId);

        if (up is null || down is null)
        {
            return null;
        }

        string upString = string.Join(',', up);
        string downString = string.Join(',', down);

        return new Team(upString, downString);
    }

    /// <summary>
    /// 验证该队伍是否有效
    /// </summary>
    /// <returns>该队伍是否有效</returns>
    public bool Validate()
    {
        return !(string.IsNullOrEmpty(UpHalf) || string.IsNullOrEmpty(DownHalf));
    }
}
