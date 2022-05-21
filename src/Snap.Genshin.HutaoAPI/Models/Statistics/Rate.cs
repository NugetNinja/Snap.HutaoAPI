// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models.Statistics;

/// <summary>
/// 比率
/// </summary>
/// <typeparam name="T">Id的类型</typeparam>
public class Rate<T>
{
    /// <summary>
    /// 构造一个新的比率
    /// </summary>
    public Rate()
    {
    }

    /// <summary>
    /// 构造一个新的比率
    /// </summary>
    /// <param name="id">id</param>
    /// <param name="value">值</param>
    public Rate(T? id, double value)
    {
        Id = id;
        Value = value;
    }

    /// <summary>
    /// Id
    /// </summary>
    public T? Id { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    public double Value { get; set; }
}
