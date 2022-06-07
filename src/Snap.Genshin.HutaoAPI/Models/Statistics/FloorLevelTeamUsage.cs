// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models.Statistics;

/// <summary>
/// 层间队伍使用
/// </summary>
public record FloorLevelTeamUsage
{
    public FloorLevelTeamUsage(FloorIndex level, IEnumerable<Rate<Team>> teams)
    {
        this.Level = level;
        this.Teams = teams;
    }

    public FloorIndex Level { get; set; }

    public IEnumerable<Rate<Team>> Teams { get; set; }

    /// <summary>
    /// 减少队伍个数
    /// 限制到x个内
    /// </summary>
    /// <param name="count">限制的个数</param>
    /// <returns>限制后的队伍使用率</returns>
    public FloorLevelTeamUsage ReduceTeamsTo(int count)
    {
        return new FloorLevelTeamUsage(Level, Teams.OrderByDescending(team => team.Value).Take(count));
    }
}
