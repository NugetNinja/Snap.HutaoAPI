// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

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
        IServiceProvider serviceProvider)
    {
        this.logger = logger;
        calculatorTypes = options.CalculatorTypes;
        this.serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 计算统计数据
    /// </summary>
    /// <returns>Task</returns>
    public async Task CalculateStatistics()
    {
        logger.LogInformation("开始计算统计数据...");

        IEnumerable<IStatisticCalculator> calculators = calculatorTypes
            .Select(type => serviceProvider.GetRequiredService(type) as IStatisticCalculator)
            .Where(x => x is not null)!;

        Stopwatch? stopWatch = new();

        foreach (IStatisticCalculator calculator in calculators)
        {
            // every calculator should not start parallel
            stopWatch.Restart();
            await calculator.Calculate();
            stopWatch.Stop();

            logger.LogInformation("计算: {type}完成，用时{time}ms。", calculator.GetType().Name, stopWatch.Elapsed.TotalMilliseconds);
        }

        logger.LogInformation("统计数据计算完毕。");
    }
}
