// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models.Identity;

/// <summary>
/// 用户
/// </summary>
public interface IUser
{
    /// <summary>
    /// UUID
    /// </summary>
    Guid UniqueUserId { get; }

    /// <summary>
    /// 获取用户信息
    /// </summary>
    /// <returns>用户信息表单</returns>
    IDictionary<string, string> GetUserInfo();
}
