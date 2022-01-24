using Microsoft.AspNetCore.Mvc;

namespace Snap.Genshin.Website.Models
{
    public static class ControllerExtension
    {
        public static ApiResponse<T> Success<T>(this ControllerBase _, string msg, T data) 
            => new(ApiCode.Success, msg, data);

        public static ApiResponse<object> Success(this ControllerBase _, string msg) 
            => new(ApiCode.Success, msg, Array.Empty<object>());

        public static ApiResponse<object> Fail(this ControllerBase _, string msg) 
            => new(ApiCode.Fail, msg, Array.Empty<object>());
    }
}
