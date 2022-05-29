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
public class AvatarParticipationCalculator : IStatisticCalculator
{
    private readonly ApplicationDbContext dbContext;
    private readonly IStatisticsProvider statisticsProvider;

    /// <summary>
    /// 构造一个新的角色搭配计算器
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="statisticsProvider">统计提供器</param>
    public AvatarParticipationCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
    {
        this.dbContext = dbContext;
        this.statisticsProvider = statisticsProvider;
    }

    /// <inheritdoc/>
    public async Task Calculate()
    {
        ConcurrentBag<AvatarParticipation> calculationResult = dbContext.SpiralAbyssAvatars
            .Where(avatar => avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex >= 9) // 忽略九层以下数据
            .Where(avatar => avatar.SpiralAbyssBattle.AbyssLevel.Star == 3) // 忽略非满星数据
            .Include(avatar => avatar.SpiralAbyssBattle)
            .ThenInclude(battle => battle.AbyssLevel)
            .AsNoTracking()
            .ParallelToMappedBag(avatar => avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex, avatar => avatar) // 按楼层分组
            .ParallelSelect(floorAvatarBarPair => new AvatarParticipation()
            {
                Floor = floorAvatarBarPair.Key,
                AvatarUsage = floorAvatarBarPair.Value
                    .ParallelToAggregateMap(avatar => avatar.AvatarId) // 统计角色的出场次数
                    .ParallelSelect(idCount => new Rate<int>(idCount.Key, (decimal)idCount.Value / floorAvatarBarPair.Value.Count)),
            });

        await statisticsProvider.SaveStatistics<AvatarParticipationCalculator>(calculationResult);
    }
}
