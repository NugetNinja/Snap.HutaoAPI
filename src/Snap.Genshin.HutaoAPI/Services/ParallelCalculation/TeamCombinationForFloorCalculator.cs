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
public class TeamCombinationForFloorCalculator : StatisticCalculator<IEnumerable<FloorTeamUsage>>
{
    private readonly ApplicationDbContext dbContext;

    /// <summary>
    /// 构造一个新的队伍出场计算器
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="statisticsProvider">统计提供器</param>
    public TeamCombinationForFloorCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        : base(statisticsProvider)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc/>
    public override IEnumerable<FloorTeamUsage> Calculate()
    {
        return dbContext.SpiralAbyssBattles
            .Where(battle => battle.AbyssLevel.FloorIndex >= 9)
            .Include(battle => battle.AbyssLevel)
            .Include(battle => battle.Avatars)
            .AsNoTracking()
            .AsEnumerable()
            .ParallelGroupBy(battle => battle.AbyssLevel.FloorIndex)
            .ParallelSelect(groupedIdBattle =>
            {
                ConcurrentBag<Rate<Team>> teamRate = groupedIdBattle.Value
                    .ParallelGroupBy(battle => battle.SpiralAbyssLevelId)
                    .ParallelSelect(idBattle => Team.FromBattleInfo(idBattle.Value))
                    .NotNull()
                    .ParallelToAggregateMap()
                    .ParallelSelect(teamCount => new Rate<Team>(teamCount));

                return new FloorTeamUsage(groupedIdBattle.Key, teamRate);
            });
    }
}