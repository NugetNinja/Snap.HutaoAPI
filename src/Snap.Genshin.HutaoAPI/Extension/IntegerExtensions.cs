// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Extension;

/// <summary>
/// 整形高性能扩展
/// </summary>
public static class IntegerExtensions
{
    /// <summary>
    /// 计算给定整数的位数
    /// </summary>
    /// <param name="x">给定的整数</param>
    /// <returns>位数</returns>
    public static int Place(this int x)
    {
        // Benchmarked and compared as a most optimized solution
        return (int)(MathF.Log10(x) + 1);
    }
}
