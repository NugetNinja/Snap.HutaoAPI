// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Extension;
using Snap.HutaoAPI.Models.Statistics;
using Snap.HutaoAPI.Services.Abstraction;

namespace Snap.HutaoAPI.Services.ParallelCalculation;

/// <summary>
/// 角色搭配计算器
/// </summary>
public class AvatarParticipationCalculator : StatisticCalculator<IEnumerable<AvatarParticipation>>
{
    private readonly ApplicationDbContext dbContext;

    /// <summary>
    /// 构造一个新的角色搭配计算器
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="statisticsProvider">统计提供器</param>
    public AvatarParticipationCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        : base(statisticsProvider)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc/>
    public override IEnumerable<AvatarParticipation> Calculate()
    {
        return dbContext.SpiralAbyssAvatars
            .Where(avatar => avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex >= 9) // 忽略九层以下数据
            .Where(avatar => avatar.SpiralAbyssBattle.AbyssLevel.Star == 3) // 忽略非满星数据
            .Include(avatar => avatar.SpiralAbyssBattle)
            .ThenInclude(battle => battle.AbyssLevel)
            .AsNoTracking()
            .AsEnumerable()
            .ParallelGroupBy(avatar => avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex, avatar => avatar) // 按楼层分组
            .ParallelSelect(floorAvatarPair => new AvatarParticipation()
            {
                Floor = floorAvatarPair.Key,
                AvatarUsage = floorAvatarPair.Value
                    .ParallelCountBy(avatar => avatar.AvatarId) // 统计角色的出场次数
                    .OrderByDescending(counter => counter.Value)
                    .ParallelSelect(idCount => new Rate<int>(idCount.Key, (decimal)idCount.Value / floorAvatarPair.Value.Count)),
            });
    }
}
