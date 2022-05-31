// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models.Statistics;

public record LevelTeamUsage
{
    public LevelTeamUsage(FloorIndex level, IEnumerable<Rate<Team>> teams)
    {
        this.Level = level;
        this.Teams = teams;
    }

    public FloorIndex Level { get; set; }

    public IEnumerable<Rate<Team>> Teams { get; set; }

    /// <summary>
    /// 减少队伍个数
    /// 限制到24个内
    /// </summary>
    /// <param name="count">限制的个数</param>
    /// <returns>限制后的队伍使用率</returns>
    public LevelTeamUsage ReduceTeamsTo(int count)
    {
        return new LevelTeamUsage(Level, Teams.OrderByDescending(team => team.Value).Take(count));
    }
}

public record FloorIndex(int Floor, int Index);
