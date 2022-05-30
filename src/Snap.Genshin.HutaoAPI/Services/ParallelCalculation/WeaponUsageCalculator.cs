// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Extension;
using Snap.HutaoAPI.Models.Statistics;
using Snap.HutaoAPI.Services.Abstraction;

namespace Snap.HutaoAPI.Services.ParallelCalculation;

/// <summary>
/// 武器使用率计数器
/// </summary>
public class WeaponUsageCalculator : StatisticCalculator<IEnumerable<WeaponUsage>>
{
    private readonly ApplicationDbContext dbContext;

    /// <summary>
    /// 构造一个新的武器使用率计算器
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="statisticsProvider">统计提供器</param>
    public WeaponUsageCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        : base(statisticsProvider)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc/>
    public override IEnumerable<WeaponUsage> Calculate()
    {
        return dbContext.AvatarDetails
            .AsNoTracking()
            .AsEnumerable()
            .ParallelGroupBy(avatar => avatar.AvatarId)
            .ParallelSelect(avatarGroup => new WeaponUsage()
            {
                Avatar = avatarGroup.Key,
                Weapons = avatarGroup.Value
                    .ParallelGroupBy(avatar => avatar.WeaponId)
                    .OrderByDescending(group => group.Value.Count)
                    .Take(8)
                    .Select(weapon => new Rate<int>
                    {
                        Id = weapon.Key,
                        Value = (decimal)weapon.Value.Count / avatarGroup.Value.Count,
                    }),
            });
    }
}