// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Snap.Genshin.MapReduce;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Models.Statistics;
using Snap.HutaoAPI.Services.Abstraction;
using System.Collections.Concurrent;

namespace Snap.HutaoAPI.Services.MapReduceCalculation;

/// <summary>
/// use map reduce
/// </summary>
public class AvatarParticipationCalculator : IStatisticCalculator
{
    private readonly ApplicationDbContext dbContext;
    private readonly IStatisticsProvider statisticsProvider;

    /// <summary>
    /// 构造一个新的角色参与计算器
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

            // 忽略 九层以下 非满星数据 数据
            .Where(avatar => avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex >= 9)
            .Where(avatar => avatar.SpiralAbyssBattle.AbyssLevel.Star == 3)

            .Include(avatar => avatar.SpiralAbyssBattle)
            .ThenInclude(battle => battle.AbyssLevel)
            .AsNoTracking()

            // 按楼层分组
            .ParallelToMappedBag(avatar => avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex, avatar => avatar)
            .ParallelSelect(floorAvatarBarPair => new AvatarParticipation()
            {
                Floor = floorAvatarBarPair.Key,
                AvatarUsage = floorAvatarBarPair.Value

                    // 统计角色的出场次数
                    .ParallelToAggregateMap(avatar => avatar.AvatarId)
                    .ParallelSelect(idCount => new Rate<int>(idCount.Key, (double)idCount.Value / floorAvatarBarPair.Value.Count)),
            });

        await statisticsProvider.SaveStatistics<AvatarParticipationCalculator>(calculationResult);
    }
}
