// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Snap.Genshin.MapReduce;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Extension;
using Snap.HutaoAPI.Models.Statistics;
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
            .Reduce((AvatarConstellationPair input, ConcurrentDictionary<int, ConcurrentBag<int>> result) =>
            {
                result
                    .GetOrNew(input.AvatarId)
                    .Add(input.Constellation);
            })
            .Reduce((KeyValuePair<int, ConcurrentBag<int>> idCountBagPair, ConcurrentBag<AvatarConstellationInfo> result) =>
            {
                IEnumerable<Rate<int>> rate = idCountBagPair.Value
                    .Reduce((int constellation, ConcurrentDictionary<int, int> constellationCountMap) =>
                    {
                        constellationCountMap.AddOrUpdate(constellation, 1, (_, count) => Interlocked.Increment(ref count));
                    })
                    .Select(idCount => new Rate<int>(idCount.Key, (double)idCount.Value / idCountBagPair.Value.Count));

                result.Add(new()
                {
                    Avatar = idCountBagPair.Key,
                    Rate = rate,
                    HoldingRate = (double)idCountBagPair.Value.Count / totalPlayerCount,
                });
            });

        await statisticsProvider.SaveStatistics<ActivedConstellationNumCalculator>(calculationResult);
    }
}

internal record AvatarConstellationPair(int AvatarId, int Constellation);
