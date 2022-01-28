using Microsoft.AspNetCore.Mvc;

namespace Snap.Genshin.Website.Models
{
    public static class ControllerExtension
    {
        public static IActionResult Success<T>(this ControllerBase _, string msg, T data)
            => new JsonResult(new ApiResponse<T>(ApiCode.Success, msg, data));

        public static IActionResult Success(this ControllerBase _, string msg)
            => new JsonResult(new ApiResponse<object>(ApiCode.Success, msg, Array.Empty<object>()));

        public static IActionResult Fail(this ControllerBase _, string msg)
            => new JsonResult(new ApiResponse<object>(ApiCode.Fail, msg, Array.Empty<object>()));

        public static IActionResult Fail(this ControllerBase _, ApiCode code, string msg)
            => new JsonResult(new ApiResponse<object>(code, msg, Array.Empty<object>()));
    }
}
