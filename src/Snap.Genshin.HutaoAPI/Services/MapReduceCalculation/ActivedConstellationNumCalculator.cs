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
/// 命座持有率计算器
/// </summary>
public class ActivedConstellationNumCalculator : IStatisticCalculator
{
    private readonly ApplicationDbContext dbContext;
    private readonly IStatisticsProvider statisticsProvider;

    /// <summary>
    /// 构造一个新的命座持有率计算器
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="statisticsProvider">统计提供器</param>
    public ActivedConstellationNumCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
    {
        this.dbContext = dbContext;
        this.statisticsProvider = statisticsProvider;
    }

    /// <inheritdoc/>
    public async Task Calculate()
    {
        int totalPlayerCount = dbContext.Players.Count();

        ConcurrentBag<AvatarConstellationInfo> calculationResult = dbContext.AvatarDetails
            .Select(avatar => new AvatarConstellationPair(avatar.AvatarId, avatar.ActivedConstellationNum))
            .AsNoTracking()

            // 按角色id分组
            .ParallelToMappedBag(input => input.AvatarId, input => input.Constellation)
            .ParallelSelect(group => new AvatarConstellationInfo()
            {
                Avatar = group.Key,
                Rate = group.Value

                    // 统计各个命座个数
                    .ParallelToAggregateMap()
                    .ParallelSelect(idCount => new Rate<int>(idCount.Key, (double)idCount.Value / group.Value.Count)),
                HoldingRate = (double)group.Value.Count / totalPlayerCount,
            });

        await statisticsProvider.SaveStatistics<ActivedConstellationNumCalculator>(calculationResult);
    }

    internal record AvatarConstellationPair(int AvatarId, int Constellation);
}