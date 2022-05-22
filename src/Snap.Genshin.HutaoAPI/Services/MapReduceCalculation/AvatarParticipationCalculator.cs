// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Snap.Genshin.MapReduce;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Entities.Record;
using Snap.HutaoAPI.Extension;
using Snap.HutaoAPI.Models.Statistics;
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

            // 忽略九层以下数据
            .Where(avatar => avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex >= 9)

            // 忽略非满星数据
            .Where(avatar => avatar.SpiralAbyssBattle.AbyssLevel.Star == 3)

            .Include(avatar => avatar.SpiralAbyssBattle)
            .ThenInclude(battle => battle.AbyssLevel)
            .AsNoTracking()
            .Reduce((SpiralAbyssAvatar avatar, ConcurrentDictionary<int, ConcurrentBag<SpiralAbyssAvatar>> result) =>
            {
                result
                    .GetOrNew(avatar.SpiralAbyssBattle.AbyssLevel.FloorIndex)
                    .Add(avatar);
            })
            .Reduce((KeyValuePair<int, ConcurrentBag<SpiralAbyssAvatar>> floorAvatarBarPair, ConcurrentBag<AvatarParticipation> result) =>
            {
                // 当前层出场的所有角色
                IEnumerable<Rate<int>>? rate = floorAvatarBarPair.Value
                    .Reduce((SpiralAbyssAvatar avatar, ConcurrentDictionary<int, int> avatarIdCountMap) =>
                    {
                        avatarIdCountMap.AddOrUpdate(avatar.AvatarId, 1, (_, count) => Interlocked.Increment(ref count));
                    })
                    .Select(idCount => new Rate<int>(idCount.Key, (double)idCount.Value / floorAvatarBarPair.Value.Count));

                result.Add(new()
                {
                    Floor = floorAvatarBarPair.Key,
                    AvatarUsage = rate,
                });
            });

        await statisticsProvider.SaveStatistics<AvatarParticipationCalculator>(calculationResult);
    }
}
