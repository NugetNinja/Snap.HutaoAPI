// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models
{
    /// <summary>
    /// ApiCode
    /// </summary>
    /// TODO: 建议使用公用仓库来对所有的apicode进行统一
    public enum ApiCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 0,

        /// <summary>
        /// 失败
        /// </summary>
        Fail = -1,

        /// <summary>
        /// 数据库异常
        /// </summary>
        DbException = 101,

        /// <summary>
        /// 服务冲突
        /// </summary>
        ServiceConflict = 102,
    }
}
