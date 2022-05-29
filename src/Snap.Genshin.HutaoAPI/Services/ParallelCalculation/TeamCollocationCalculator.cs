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
/// 角色搭配计算器
/// </summary>
public class TeamCollocationCalculator : IStatisticCalculator
{
    private readonly ApplicationDbContext dbContext;
    private readonly IStatisticsProvider statisticsProvider;

    /// <summary>
    /// 构造一个新的角色搭配计算器
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="statisticsProvider">统计提供器</param>
    public TeamCollocationCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
    {
        this.dbContext = dbContext;
        this.statisticsProvider = statisticsProvider;
    }

    /// <inheritdoc/>
    public async Task Calculate()
    {
        ConcurrentBag<TeamCollocation> calculationResult = dbContext.SpiralAbyssBattles
            .Include(battle => battle.Avatars)
            .Select(battle => battle.Avatars)
            .AsNoTracking()
            .AsParallel()
            .ParallelSelect(list => list.Select(avatar => avatar.AvatarId)) // 转换到仅剩id的列表
            .ParallelSelect(ids => ids.Select(id => new AvatarBattleWith(id, ids.Where(a => a != id))))
            .SelectMany(list => list)
            .ParallelToMappedBag(battle => battle.AvatarId, battle => battle.BattleWith)
            .ParallelSelect(avatarBattleWith =>
            {
                IEnumerable<int> flattenAvatars = avatarBattleWith.Value
                    .SelectMany(id => id);
                decimal totalAvatarBattleWithCount = flattenAvatars.Count();
                return new TeamCollocation
                {
                    Avatar = avatarBattleWith.Key,
                    Collocations = flattenAvatars
                        .ParallelToAggregateMap()
                        .Select(idCount => new Rate<int>(idCount.Key, idCount.Value / totalAvatarBattleWithCount)),
                };
            });

        await statisticsProvider.SaveStatistics<TeamCollocationCalculator>(calculationResult);
    }

    private class AvatarBattleWith
    {
        public AvatarBattleWith(int avatarId, IEnumerable<int> battleWith)
        {
            AvatarId = avatarId;
            BattleWith = battleWith;
        }

        public int AvatarId { get; set; }

        public IEnumerable<int> BattleWith { get; set; }
    }
}
