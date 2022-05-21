// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

namespace Snap.HutaoAPI.Models.Identity
{
    /// <summary>
    /// 邮件
    /// </summary>
    public interface IMail
    {
        /// <summary>
        /// 邮件标题
        /// </summary>
        string Title { get; }

        /// <summary>
        /// 邮件内容
        /// </summary>
        string Content { get; }

        /// <summary>
        /// 收件人
        /// </summary>
        string Destination { get; }

        /// <summary>
        /// 发件人
        /// </summary>
        string Sender { get; }
    }
}
