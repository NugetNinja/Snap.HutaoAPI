// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models.Identity
{
    /// <summary>
    /// 身份组策略名称
    /// </summary>
    public static class IdentityPolicyNames
    {
        /// <summary>
        /// 普通用户
        /// </summary>
        public const string CommonUser = nameof(CommonUser);

        /// <summary>
        /// 管理员
        /// </summary>
        public const string Administrator = nameof(Administrator);

        /// <summary>
        /// 刷新令牌
        /// </summary>
        public const string RefreshTokenOnly = nameof(RefreshTokenOnly);
    }
}
