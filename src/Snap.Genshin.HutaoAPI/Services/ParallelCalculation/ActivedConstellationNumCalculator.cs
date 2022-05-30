// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Extension;
using Snap.HutaoAPI.Models.Statistics;
using Snap.HutaoAPI.Services.Abstraction;

namespace Snap.HutaoAPI.Services.ParallelCalculation;

/// <summary>
/// 命座持有率计算器
/// </summary>
public class ActivedConstellationNumCalculator : StatisticCalculator<IEnumerable<AvatarConstellationInfo>>
{
    private readonly ApplicationDbContext dbContext;

    /// <summary>
    /// 构造一个新的命座持有率计算器
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="statisticsProvider">统计提供器</param>
    public ActivedConstellationNumCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        : base(statisticsProvider)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc/>
    public override IEnumerable<AvatarConstellationInfo> Calculate()
    {
        decimal totalPlayerCount = dbContext.Players.Count();

        return dbContext.AvatarDetails
            .Select(avatar => new AvatarConstellationPair(avatar.AvatarId, avatar.ActivedConstellationNum))
            .AsNoTracking()
            .AsEnumerable()
            .ParallelToMappedBag(input => input.AvatarId, input => input.Constellation) // 按角色id分组
            .ParallelSelect(group => new AvatarConstellationInfo()
            {
                Avatar = group.Key,
                Rate = group.Value
                    .ParallelToAggregateMap() // 统计各个命座个数
                    .ParallelSelect(idCount => new Rate<int>(idCount.Key, (decimal)idCount.Value / group.Value.Count)),
                HoldingRate = group.Value.Count / totalPlayerCount,
            });
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