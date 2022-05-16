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
        Success = 0,
        Fail = -1,
        // 数据库异常
        DbException = 101,
        // 服务冲突
        ServiceConcurrent = 102,
    }
}
