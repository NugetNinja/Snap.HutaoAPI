// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Helper;

/// <summary>
/// 类型工厂
/// </summary>
public static class Types
{
    /// <summary>
    /// 委托构造一个新的 <typeparamref name="TReturn"/> 类型的实例
    /// </summary>
    /// <typeparam name="TReturn">返回类型</typeparam>
    /// <returns>实例</returns>
    public static TReturn New<TReturn>()
        where TReturn : new()
    {
        return new TReturn();
    }

    /// <summary>
    /// 委托构造一个新的 <typeparamref name="TReturn"/> 类型的实例
    /// </summary>
    /// <typeparam name="T1">丢弃类型</typeparam>
    /// <typeparam name="TReturn">返回类型</typeparam>
    /// <param name="t1">丢弃，仅用于匹配委托</param>
    /// <returns>实例</returns>
    public static TReturn New<T1, TReturn>(T1 t1)
        where TReturn : new()
    {
        return new TReturn();
    }
}
