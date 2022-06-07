// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Extensions.Caching.Memory;
using Snap.HutaoAPI.Configurations;
using Snap.HutaoAPI.Services.Abstraction;
using System.Diagnostics;

namespace Snap.HutaoAPI.Services;

/// <summary>
/// GenshinStatisticsService
/// </summary>
public class GenshinStatisticsService
{
    private readonly ILogger logger;
    private readonly IMemoryCache memoryCache;
    private readonly List<Type> calculatorTypes;
    private readonly IServiceProvider serviceProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="GenshinStatisticsService"/> class.
    /// </summary>
    /// <param name="options">op from di</param>
    /// <param name="logger">logger from di</param>
    /// <param name="serviceProvider">service from di</param>
    public GenshinStatisticsService(
        GenshinStatisticsServiceConfiguration options,
        ILogger<GenshinStatisticsService> logger,
        IMemoryCache memoryCache,
        IServiceProvider serviceProvider)
    {
        this.logger = logger;
        this.memoryCache = memoryCache;
        calculatorTypes = options.CalculatorTypes;
        this.serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 计算统计数据
    /// </summary>
    /// <returns>Task</returns>
    public async Task CalculateAllStatistics()
    {
        memoryCache.Set("_STATISTICS_BUSY", true);
        logger.LogInformation("开始计算统计数据...");

        IEnumerable<IStatisticPipeline> pipelines = calculatorTypes
            .Select(type => serviceProvider.GetRequiredService(type) as IStatisticPipeline)
            .Where(x => x is not null)!;

        Stopwatch? stopWatch = new();

        // every calculator should NOT start parallel cause dbcontext is not a thread safe class
        foreach (IStatisticPipeline pipeline in pipelines)
        {
            stopWatch.Restart();
            await pipeline.CalculateAndSaveAsync();
            stopWatch.Stop();

            logger.LogInformation("计算: {type} 完成，用时 {time} ms。", pipeline.GetType().Name, stopWatch.Elapsed.TotalMilliseconds);
        }

        logger.LogInformation("统计数据计算完毕。");
        memoryCache.Remove("_STATISTICS_BUSY");
    }
}
