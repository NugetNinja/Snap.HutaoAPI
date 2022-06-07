// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models.Statistics;

/// <summary>
/// 层队伍使用
/// </summary>
public record FloorTeamUsage
{
    public FloorTeamUsage(int floor, IEnumerable<Rate<Team>> teams)
    {
        this.Floor = floor;
        this.Teams = teams;
    }

    public int Floor { get; set; }

    public IEnumerable<Rate<Team>> Teams { get; set; }

    /// <summary>
    /// 减少队伍个数
    /// 限制到x个内
    /// </summary>
    /// <param name="count">限制的个数</param>
    /// <returns>限制后的队伍使用率</returns>
    public FloorTeamUsage ReduceTeamsTo(int count)
    {
        return new FloorTeamUsage(Floor, Teams.OrderByDescending(team => team.Value).Take(count));
    }
}