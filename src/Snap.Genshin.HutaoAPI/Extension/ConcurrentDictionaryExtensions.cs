// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.HutaoAPI.Helper;
using System.Collections.Concurrent;

namespace Snap.HutaoAPI.Extension;

/// <summary>
/// 并行字典扩展
/// </summary>
public static class ConcurrentDictionaryExtensions
{
    /// <summary>
    /// 获取或新建键对应的项
    /// </summary>
    /// <typeparam name="TKey">键的类型</typeparam>
    /// <typeparam name="TValue">值的类型</typeparam>
    /// <param name="dict">字典</param>
    /// <param name="key">键</param>
    /// <returns>原本存在或新创建的值</returns>
    public static TValue GetOrNew<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key)
        where TValue : class, new()
    {
        return dict.GetOrAdd(key, Types.New<TKey, TValue>);
    }
}
