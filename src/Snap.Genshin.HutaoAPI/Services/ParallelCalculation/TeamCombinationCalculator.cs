// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Entities.Record;
using Snap.HutaoAPI.Extension;
using Snap.HutaoAPI.Models.Statistics;
using Snap.HutaoAPI.Services.Abstraction;
using System.Collections.Concurrent;

namespace Snap.HutaoAPI.Services.ParallelCalculation;

/// <summary>
/// 队伍出场计算器
/// </summary>
public class TeamCombinationCalculator : IStatisticCalculator
{
    private readonly ApplicationDbContext dbContext;
    private readonly IStatisticsProvider statisticsProvider;

    /// <summary>
    /// 构造一个新的队伍出场计算器
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="statisticsProvider">统计提供器</param>
    public TeamCombinationCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
    {
        this.dbContext = dbContext;
        this.statisticsProvider = statisticsProvider;
    }

    public async Task Calculate()
    {
        ConcurrentBag<LevelTeamUsage> calcaulationResult = dbContext.SpiralAbyssBattles
            .Where(battle => battle.AbyssLevel.FloorIndex >= 9)
            .Include(battle => battle.AbyssLevel)
            .AsNoTracking()
            .ParallelGroupBy(battle => new FloorIndex(battle.AbyssLevel.FloorIndex, battle.AbyssLevel.LevelIndex))
            .ParallelSelect(groupedIdBattle =>
            {
                ConcurrentBag<Rate<Team>> teamRate = groupedIdBattle.Value
                    .ParallelGroupBy(battle => battle.SpiralAbyssLevelId)
                    .ParallelSelect(idBattle =>
                    {
                        IList<SpiralAbyssAvatar>? upHalfAvatars = idBattle.Value
                            .Where(battle => battle.BattleIndex == 1)
                            .Select(battle => battle.Avatars).SingleOrDefault();
                        IList<SpiralAbyssAvatar>? downHalfAvatars = idBattle.Value
                            .Where(battle => battle.BattleIndex == 2)
                            .Select(battle => battle.Avatars).SingleOrDefault();

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

                        return new Team(upHalfAvatarsString, downHalfAvatarsString);
                    })
                    .Where(team => team.Validate())
                    .ParallelToAggregateMap()
                    .OrderByDescending(countTeam => countTeam.Value)
                    .Take(24)
                    .ParallelSelect(teamCount => new Rate<Team>(teamCount));

                return new LevelTeamUsage(groupedIdBattle.Key, teamRate);
            });

        await statisticsProvider.SaveStatistics<TeamCombinationCalculator>(calcaulationResult);
    }
}