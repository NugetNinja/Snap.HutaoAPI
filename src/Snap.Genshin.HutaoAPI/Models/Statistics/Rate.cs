// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json.Serialization;

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
    /// <param name="id">id</param>
    /// <param name="value">值</param>
    [JsonConstructor]
    public Rate(T id, decimal value)
    {
        Id = id;
        Value = value;
    }

    /// <summary>
    /// 构造一个新的比率
    /// </summary>
    /// <param name="keyValuePair">键值对</param>
    public Rate(KeyValuePair<T, int> keyValuePair)
    {
        Id = keyValuePair.Key;
        Value = keyValuePair.Value;
    }

    /// <summary>
    /// Id
    /// </summary>
    public T Id { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    public decimal Value { get; set; }
}
