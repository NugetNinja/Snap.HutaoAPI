// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json.Serialization;

namespace Snap.HutaoAPI.Models
{

    /// <summary>
    /// 带有数据的响应
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public class ApiResponse<T> : ApiResponse
    {
        /// <summary>
        /// 构造一个新的响应
        /// </summary>
        /// <param name="code">响应代码</param>
        /// <param name="message">消息</param>
        /// <param name="data">数据</param>
        public ApiResponse(ApiCode code, string message, T data)
            : base(code, message)
        {
            Data = data;
        }

        /// <summary>
        /// 数据
        /// </summary>
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
