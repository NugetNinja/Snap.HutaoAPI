// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Entities.Record;
using Snap.HutaoAPI.Extension;
using Snap.HutaoAPI.Models.Statistics;
using Snap.HutaoAPI.Services.Abstraction;
using System.Collections.Concurrent;

namespace Snap.HutaoAPI.Services.ParallelCalculation;

/// <summary>
/// 角色圣遗物搭配计算器
/// </summary>
public class AvatarReliquaryUsageCalculator : IStatisticCalculator
{
    private readonly ApplicationDbContext dbContext;
    private readonly IStatisticsProvider statisticsProvider;

    /// <summary>
    /// 构造一个新的圣遗物搭配计算器
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="statisticsProvider">统计提供器</param>
    public AvatarReliquaryUsageCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
    {
        this.dbContext = dbContext;
        this.statisticsProvider = statisticsProvider;
    }

    /// <inheritdoc/>
    public async Task Calculate()
    {
        ConcurrentBag<AvatarReliquaryUsage> calculationResult = dbContext.AvatarDetails
            .Include(avatar => avatar.ReliquarySets)
            .AsNoTracking()
            .ParallelToMappedBag(
                avatar => avatar.AvatarId,
                avatar => avatar.GetNormalizedReliquarySets(),
                sets => sets.Any())
            .ParallelSelect(input =>
            {
                ConcurrentBag<string>? relicBag = input.Value
                    .ParallelSelect(ConvertReliquarySetsToString);

                // 提取有效圣遗物总数
                decimal totalCount = relicBag.Count;

                IEnumerable<Rate<string>> rates = relicBag
                    .ParallelToAggregateMap()
                    .OrderByDescending(relicSetCount => relicSetCount.Value)
                    .Take(8)
                    .Select(relicSetCount => new Rate<string>(relicSetCount.Key, relicSetCount.Value / totalCount));

                return new AvatarReliquaryUsage(input.Key, rates);
            });

        await statisticsProvider.SaveStatistics<AvatarReliquaryUsageCalculator>(calculationResult);
    }

    private string ConvertReliquarySetsToString(IList<DetailedReliquarySetInfo> sets)
    {
        return string.Join(';', sets.Select(set => set.UnionId));
    }
}
