// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Extension;
using Snap.HutaoAPI.Models.Statistics;
using Snap.HutaoAPI.Services.Abstraction;
using System.Collections.Concurrent;

namespace Snap.HutaoAPI.Services.ParallelCalculation;

/// <summary>
/// 队伍出场计算器
/// </summary>
public class TeamCombinationCalculator : StatisticCalculator<IEnumerable<LevelTeamUsage>>
{
    private readonly ApplicationDbContext dbContext;

    /// <summary>
    /// 构造一个新的队伍出场计算器
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="statisticsProvider">统计提供器</param>
    public TeamCombinationCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        : base(statisticsProvider)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc/>
    public override IEnumerable<LevelTeamUsage> Calculate()
    {
        return dbContext.SpiralAbyssBattles
            .Where(battle => battle.AbyssLevel.FloorIndex >= 9)
            .Include(battle => battle.AbyssLevel)
            .AsNoTracking()
            .AsEnumerable()
            .ParallelGroupBy(battle => new FloorIndex(battle.AbyssLevel.FloorIndex, battle.AbyssLevel.LevelIndex))
            .ParallelSelect(groupedIdBattle =>
            {
                ConcurrentBag<Rate<Team>> teamRate = groupedIdBattle.Value
                    .ParallelGroupBy(battle => battle.SpiralAbyssLevelId)
                    .ParallelSelect(idBattle => Team.FromBattleInfo(idBattle.Value))
                    .NotNull()
                    .ParallelToAggregateMap()
                    .OrderByDescending(countTeam => countTeam.Value)
                    .Take(24)
                    .ParallelSelect(teamCount => new Rate<Team>(teamCount));

                return new LevelTeamUsage(groupedIdBattle.Key, teamRate);
            });
    }
}