// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Snap.HutaoAPI.Controllers;

/// <summary>
/// 验证接口控制器
/// </summary>
[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="request">请求模型</param>
    /// <returns>Ok-成功 Unauthorized-密码错误 BadRequest-用户不存在</returns>
    [AllowAnonymous]
    [HttpPost("[Action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Login([FromBody] LoginModel request)
    {
        return RedirectPreserveMethod("https://auth.snapgenshin.com/auth/login");
    }

    public record LoginModel([property:Required] Guid AppId, [property:Required] string Secret);
}
