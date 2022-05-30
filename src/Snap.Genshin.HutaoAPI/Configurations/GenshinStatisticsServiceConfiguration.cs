// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.HutaoAPI.Services.Abstraction;

namespace Snap.HutaoAPI.Configurations;

/// <summary>
/// 统计相关配置
/// </summary>
public class GenshinStatisticsServiceConfiguration
{
    /// <summary>
    /// 计算器的类型集合
    /// </summary>
    public List<Type> CalculatorTypes { get; } = new();

    /// <summary>
    /// 添加计算器
    /// </summary>
    /// <typeparam name="T">计算器的类型</typeparam>
    /// <returns>可继续操作的配置</returns>
    public GenshinStatisticsServiceConfiguration AddCalculator<T>()
        where T : IStatisticPipeline
    {
        CalculatorTypes.Add(typeof(T));
        return this;
    }
}
