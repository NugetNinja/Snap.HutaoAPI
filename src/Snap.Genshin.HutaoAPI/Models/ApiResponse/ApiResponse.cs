// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Text.Json.Serialization;

namespace Snap.HutaoAPI.Models
{
    /// <summary>
    /// ApiResponse
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="code">Apicode</param>
        /// <param name="message">msg</param>
        /// <param name="data">data</param>
        public ApiResponse(ApiCode code, string message, T data)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        /// <summary>
        /// ApiCode
        /// </summary>
        [JsonPropertyName("retcode")]
        public ApiCode Code { get; set; }

        /// <summary>
        /// Msg
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        [JsonPropertyName("data")]
        public T? Data { get; set; }
    }
}
