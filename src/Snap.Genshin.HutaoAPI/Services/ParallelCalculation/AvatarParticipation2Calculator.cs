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
/// 角色使用率计算器
/// </summary>
public class AvatarParticipation2Calculator : StatisticCalculator<IEnumerable<AvatarParticipation>>
{
    private readonly ApplicationDbContext dbContext;

    /// <summary>
    /// 构造一个新的角色使用率计算器
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="statisticsProvider">统计提供器</param>
    public AvatarParticipation2Calculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        : base(statisticsProvider)
    {
        this.dbContext = dbContext;
    }

    /// <inheritdoc/>
    public override IEnumerable<AvatarParticipation> Calculate()
    {
        ConcurrentDictionary<int, int> avatarHoldCounter = dbContext.PlayerRecords
            .Include(record => record.Player)
            .ThenInclude(player => player.Avatars)
            .AsNoTracking()
            .AsEnumerable()
            .SelectMany(r => r.Player.Avatars)
            .ParallelCountBy(a => a.AvatarId);

        return dbContext.SpiralAbyssAvatars
            .Where(avatar => avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex >= 9) // 忽略九层以下数据
            .Where(avatar => avatar.SpiralAbyssBattle.AbyssLevel.Star == 3) // 忽略非满星数据
            .Include(avatar => avatar.SpiralAbyssBattle)
            .ThenInclude(battle => battle.AbyssLevel)
            .ThenInclude(level => level.Record)
            .AsNoTracking()
            .AsEnumerable()
            .ParallelGroupBy(avatar => avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex) // 按层分组
            .ParallelSelect(avatars => new AvatarParticipation()
            {
                Floor = avatars.Key,
                AvatarUsage = avatars.Value
                    .DistinctBy(a => new LevelAvater(a.SpiralAbyssBattle.AbyssLevel.RecordId, a.AvatarId)) // 同记录内仅统计1次
                    .ParallelCountBy(a => a.AvatarId)
                    .Select(avatarCount => new Rate<int>(avatarCount.Key, (decimal)avatarCount.Value / avatarHoldCounter[avatarCount.Key])),
            });
    }

    private record LevelAvater(long LevelId, int AvatarId);
}