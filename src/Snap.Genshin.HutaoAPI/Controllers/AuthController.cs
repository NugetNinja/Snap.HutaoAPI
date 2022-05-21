using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Models;
using Snap.HutaoAPI.Models.ApiRequest;
using Snap.HutaoAPI.Models.Utility;
using Snap.HutaoAPI.Services;

namespace Snap.HutaoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <returns>Ok-成功 Unauthorized-密码错误 BadRequest-用户不存在</returns>
        [AllowAnonymous]
        [HttpPost("[Action]")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Login([FromBody] LoginModel request)
        {
            return RedirectPreserveMethod("https://auth.snapgenshin.com/auth/login");
        }
    }
}
