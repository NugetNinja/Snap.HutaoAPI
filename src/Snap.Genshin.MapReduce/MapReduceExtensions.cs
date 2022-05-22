// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Concurrent;

namespace Snap.Genshin.MapReduce;

/// <summary>
/// 规约扩展方法
/// </summary>
public static class MapReduceExtensions
{
    /// <summary>
    /// 规约
    /// </summary>
    /// <typeparam name="TInput">输入的类型</typeparam>
    /// <typeparam name="TOutputKey">输出键的类型</typeparam>
    /// <typeparam name="TOutputValue">输出值的类型</typeparam>
    /// <param name="inputData">输入数据</param>
    /// <param name="action">执行的规约操作</param>
    /// <returns>规约的结果</returns>
    public static ConcurrentDictionary<TOutputKey, TOutputValue> Reduce<TInput, TOutputKey, TOutputValue>(
        this IEnumerable<TInput> inputData,
        Action<TInput, ConcurrentDictionary<TOutputKey, TOutputValue>> action)
        where TOutputKey : notnull
    {
        ConcurrentDictionary<TOutputKey, TOutputValue> result = new();
        Parallel.ForEach(inputData, input => action(input, result));
        return result;
    }

    /// <summary>
    /// 规约
    /// </summary>
    /// <typeparam name="TInput">输入的类型</typeparam>
    /// <typeparam name="TOutput">输出值的类型</typeparam>
    /// <param name="inputData">输入数据</param>
    /// <param name="action">执行的规约操作</param>
    /// <returns>规约的结果</returns>
    public static ConcurrentBag<TOutput> Reduce<TInput, TOutput>(
        this IEnumerable<TInput> inputData,
        Action<TInput, ConcurrentBag<TOutput>> action)
    {
        ConcurrentBag<TOutput> result = new();
        Parallel.ForEach(inputData, input => action(input, result));
        return result;
    }
}
