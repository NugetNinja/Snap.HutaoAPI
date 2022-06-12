// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Mvc;

namespace Snap.HutaoAPI.Models
{
    /// <summary>
    /// extension for response
    /// </summary>
    public static class ControllerBaseExtension
    {
        /// <summary>
        /// 成功
        /// </summary>
        /// <typeparam name="T">返回数据的类型</typeparam>
        /// <param name="controller">控制器</param>
        /// <param name="msg">消息</param>
        /// <param name="data">返回的数据</param>
        /// <returns>操作结果</returns>
        public static IActionResult Success<T>(this ControllerBase controller, string msg, T data)
        {
            return new JsonResult(new ApiResponse<T>(ApiCode.Success, msg, data));
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="msg">消息</param>
        /// <returns>操作结果</returns>
        public static IActionResult Success(this ControllerBase controller, string msg)
        {
            return new JsonResult(new ApiResponse(ApiCode.Success, msg));
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="msg">消息</param>
        /// <returns>操作结果</returns>
        public static IActionResult Fail(this ControllerBase controller, string msg)
        {
            return new JsonResult(new ApiResponse(ApiCode.Fail, msg));
        }

        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="controller">控制器</param>
        /// <param name="code">返回代码</param>
        /// <param name="msg">消息</param>
        /// <returns>操作结果</returns>
        public static IActionResult Fail(this ControllerBase controller, ApiCode code, string msg)
        {
            return new JsonResult(new ApiResponse(code, msg));
        }
    }
}
