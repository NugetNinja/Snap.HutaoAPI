// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Extensions.Caching.Memory;

namespace Snap.HutaoAPI.Extension;

/// <summary>
/// 内存缓存扩展
/// </summary>
public static class IMemoryCacheExtensions
{
    /// <summary>
    /// 获取内存缓存中储存的值
    /// </summary>
    /// <param name="memoryCache">内存缓存</param>
    /// <param name="key">键</param>
    /// <returns>是否定义了键</returns>
    public static bool Has(this IMemoryCache memoryCache, object key)
    {
        return memoryCache.TryGetValue(key, out _);
    }
}
