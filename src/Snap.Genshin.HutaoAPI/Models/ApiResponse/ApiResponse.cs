﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json.Serialization;

namespace Snap.HutaoAPI.Models
{
    /// <summary>
    /// 响应
    /// </summary>
    public class ApiResponse
    {
        /// <summary>
        /// 构造一个新的响应
        /// </summary>
        /// <param name="code">响应代码</param>
        /// <param name="message">消息</param>
        public ApiResponse(ApiCode code, string message)
        {
            Code = code;
            Message = message;
        }

        /// <summary>
        /// 返回代码
        /// </summary>
        [JsonPropertyName("retcode")]
        public ApiCode Code { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
