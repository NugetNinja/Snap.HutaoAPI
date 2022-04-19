using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Models;
using Snap.Genshin.Website.Models.ApiRequest;
using Snap.Genshin.Website.Models.Utility;
using Snap.Genshin.Website.Services;

namespace Snap.Genshin.Website.Controllers
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
