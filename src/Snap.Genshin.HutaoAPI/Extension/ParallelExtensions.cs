// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft;
using System.Collections.Concurrent;

namespace Snap.HutaoAPI.Extension;

/// <summary>
/// 规约扩展方法
/// </summary>
public static class ParallelExtensions
{
    /// <summary>
    /// 规约到映射
    /// </summary>
    /// <typeparam name="TInput">输入的类型</typeparam>
    /// <typeparam name="TOutputKey">输出键的类型</typeparam>
    /// <typeparam name="TOutputValue">输出值的类型</typeparam>
    /// <param name="inputData">输入数据</param>
    /// <param name="action">执行的规约操作</param>
    /// <returns>规约的结果</returns>
    public static ConcurrentDictionary<TOutputKey, TOutputValue> ParallelToMap<TInput, TOutputKey, TOutputValue>(
        this IEnumerable<TInput> inputData,
        Action<TInput, ConcurrentDictionary<TOutputKey, TOutputValue>> action)
        where TOutputKey : notnull
    {
        ConcurrentDictionary<TOutputKey, TOutputValue> result = new();
        Parallel.ForEach(inputData, input => action(input, result));
        return result;
    }

    /// <summary>
    /// 规约到映射
    /// </summary>
    /// <typeparam name="TInput">输入的类型</typeparam>
    /// <typeparam name="TOutputKey">输出键的类型</typeparam>
    /// <typeparam name="TOutputValue">输出值的类型</typeparam>
    /// <param name="inputData">输入数据</param>
    /// <param name="dictionaryFactory">字典构造器</param>
    /// <param name="action">执行的规约操作</param>
    /// <returns>规约的结果</returns>
    public static ConcurrentDictionary<TOutputKey, TOutputValue> ParallelToMap<TInput, TOutputKey, TOutputValue>(
        this IEnumerable<TInput> inputData,
        Func<ConcurrentDictionary<TOutputKey, TOutputValue>> dictionaryFactory,
        Action<TInput, ConcurrentDictionary<TOutputKey, TOutputValue>> action)
        where TOutputKey : notnull
    {
        ConcurrentDictionary<TOutputKey, TOutputValue> result = dictionaryFactory();
        Parallel.ForEach(inputData, input => action(input, result));
        return result;
    }

    /// <summary>
    /// 规约到计数映射
    /// </summary>
    /// <typeparam name="TInput">输入的类型</typeparam>
    /// <param name="inputData">输入数据</param>
    /// <returns>规约的结果</returns>
    public static ConcurrentDictionary<TInput, int> ParallelToAggregateMap<TInput>(this IEnumerable<TInput> inputData)
        where TInput : notnull
    {
        return inputData.ParallelToMap((TInput input, ConcurrentDictionary<TInput, int> countMap) =>
        {
            countMap.AddOrUpdate(input, 1, (_, count) => Interlocked.Increment(ref count));
        });
    }

    /// <summary>
    /// 规约到计数映射
    /// </summary>
    /// <typeparam name="TInput">输入的类型</typeparam>
    /// <param name="inputData">输入数据</param>
    /// <param name="mapFactory">构造一个新的字典</param>
    /// <returns>规约的结果</returns>
    public static ConcurrentDictionary<TInput, int> ParallelToAggregateMap<TInput>(this IEnumerable<TInput> inputData, Func<ConcurrentDictionary<TInput, int>> mapFactory)
        where TInput : notnull
    {
        return inputData.ParallelToMap(mapFactory, (TInput input, ConcurrentDictionary<TInput, int> countMap) =>
        {
            countMap.AddOrUpdate(input, 1, (_, count) => Interlocked.Increment(ref count));
        });
    }

    /// <summary>
    /// 规约到计数映射
    /// </summary>
    /// <typeparam name="TInput">输入的类型</typeparam>
    /// <typeparam name="TOutputKey">输出键的类型</typeparam>
    /// <param name="inputData">输入数据</param>
    /// <param name="keySelector">键选择器</param>
    /// <returns>规约的结果</returns>
    public static ConcurrentDictionary<TOutputKey, int> ParallelCountBy<TInput, TOutputKey>(
        this IEnumerable<TInput> inputData,
        Func<TInput, TOutputKey> keySelector)
        where TInput : notnull
        where TOutputKey : notnull
    {
        return inputData.ParallelToMap((TInput input, ConcurrentDictionary<TOutputKey, int> countMap) =>
        {
            countMap.AddOrUpdate(keySelector(input), 1, (_, count) => Interlocked.Increment(ref count));
        });
    }

    /// <summary>
    /// 规约到包的映射
    /// </summary>
    /// <typeparam name="TInput">输入的类型</typeparam>
    /// <typeparam name="TOutputKey">输出键的类型</typeparam>
    /// <param name="inputData">输入数据</param>
    /// <param name="keySelector">键选择器</param>
    /// <returns>规约的结果</returns>
    public static ConcurrentDictionary<TOutputKey, ConcurrentBag<TInput>> ParallelGroupBy<TInput, TOutputKey>(
        this IEnumerable<TInput> inputData,
        Func<TInput, TOutputKey> keySelector)
        where TOutputKey : notnull
    {
        return inputData.ParallelToMap((TInput input, ConcurrentDictionary<TOutputKey, ConcurrentBag<TInput>> result) =>
        {
            result
                .GetOrAdd(keySelector(input), (_) => new ConcurrentBag<TInput>())
                .Add(input);
        });
    }

    /// <summary>
    /// 规约到包的映射
    /// </summary>
    /// <typeparam name="TInput">输入的类型</typeparam>
    /// <typeparam name="TOutputKey">输出键的类型</typeparam>
    /// <typeparam name="TOutputValue">输出包内值的类型</typeparam>
    /// <param name="inputData">输入数据</param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="valueSelector">值选择器</param>
    /// <returns>规约的结果</returns>
    public static ConcurrentDictionary<TOutputKey, ConcurrentBag<TOutputValue>> ParallelGroupBy<TInput, TOutputKey, TOutputValue>(
        this IEnumerable<TInput> inputData,
        Func<TInput, TOutputKey> keySelector,
        Func<TInput, TOutputValue> valueSelector)
        where TOutputKey : notnull
    {
        return inputData.ParallelToMap((TInput input, ConcurrentDictionary<TOutputKey, ConcurrentBag<TOutputValue>> result) =>
        {
            result
                .GetOrAdd(keySelector(input), (_) => new ConcurrentBag<TOutputValue>())
                .Add(valueSelector(input));
        });
    }

    /// <summary>
    /// 规约到包的映射
    /// </summary>
    /// <typeparam name="TInput">输入的类型</typeparam>
    /// <typeparam name="TOutputKey">输出键的类型</typeparam>
    /// <typeparam name="TOutputValue">输出包内值的类型</typeparam>
    /// <param name="inputData">输入数据</param>
    /// <param name="keySelector">键选择器</param>
    /// <param name="valueSelector">值选择器</param>
    /// <param name="valueFilter">值过滤器</param>
    /// <returns>规约的结果</returns>
    public static ConcurrentDictionary<TOutputKey, ConcurrentBag<TOutputValue>> ParallelToMappedBag<TInput, TOutputKey, TOutputValue>(
        this IEnumerable<TInput> inputData,
        Func<TInput, TOutputKey> keySelector,
        Func<TInput, TOutputValue> valueSelector,
        Func<TOutputValue, bool> valueFilter)
        where TOutputKey : notnull
    {
        return inputData.ParallelToMap((TInput input, ConcurrentDictionary<TOutputKey, ConcurrentBag<TOutputValue>> result) =>
        {
            TOutputValue value = valueSelector(input);
            if (valueFilter(value))
            {
                result
                    .GetOrAdd(keySelector(input), (_) => new ConcurrentBag<TOutputValue>())
                    .Add(value);
            }
        });
    }

    /// <summary>
    /// 规约到包
    /// </summary>
    /// <typeparam name="TInput">输入的类型</typeparam>
    /// <typeparam name="TOutput">输出值的类型</typeparam>
    /// <param name="inputData">输入数据</param>
    /// <param name="valueSelector">值选择器</param>
    /// <returns>规约的结果</returns>
    public static ConcurrentBag<TOutput> ParallelSelect<TInput, TOutput>(
        this IEnumerable<TInput> inputData,
        Func<TInput, TOutput> valueSelector)
    {
        ConcurrentBag<TOutput> result = new();
        Parallel.ForEach(inputData, input => result.Add(valueSelector(input)));
        return result;
    }

    /// <summary>
    /// 非空
    /// </summary>
    /// <typeparam name="T">源类型</typeparam>
    /// <param name="source">源</param>
    /// <returns>非空的源</returns>
    public static IEnumerable<T> NotNull<T>(this IEnumerable<T?> source)
        where T : notnull
    {
        if (source.Any())
        {
            return source.Where(x => x != null)!;
        }

        return Enumerable.Empty<T>();
    }
}
