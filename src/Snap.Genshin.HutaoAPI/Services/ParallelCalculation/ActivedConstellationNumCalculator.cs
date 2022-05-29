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
        decimal totalPlayerCount = dbContext.Players.Count();

        ConcurrentBag<AvatarConstellationInfo> calculationResult = dbContext.AvatarDetails
            .Select(avatar => new AvatarConstellationPair(avatar.AvatarId, avatar.ActivedConstellationNum))
            .AsNoTracking()
            .ParallelToMappedBag(input => input.AvatarId, input => input.Constellation) // 按角色id分组
            .ParallelSelect(group => new AvatarConstellationInfo()
            {
                Avatar = group.Key,
                Rate = group.Value
                    .ParallelToAggregateMap() // 统计各个命座个数
                    .ParallelSelect(idCount => new Rate<int>(idCount.Key, (decimal)idCount.Value / group.Value.Count)),
                HoldingRate = group.Value.Count / totalPlayerCount,
            });

        await statisticsProvider.SaveStatistics<ActivedConstellationNumCalculator>(calculationResult);
    }

    private class AvatarConstellationPair
    {
        public AvatarConstellationPair(int avatarId, int constellation)
        {
            AvatarId = avatarId;
            Constellation = constellation;
        }

        public int AvatarId { get; set; }

        public int Constellation { get; set; }
    }
}