// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snap.HutaoAPI.Models;
using Snap.HutaoAPI.Models.Identity;

namespace Snap.HutaoAPI.Controllers;

/// <summary>
/// 角色展柜控制器
/// </summary>
[Route("[controller]")]
[ApiController]
public class ShowcaseController : ControllerBase
{
    /// <summary>
    /// 刷新角色展柜数据
    /// </summary>
    /// <param name="uid">用户uid</param>
    /// <returns>是否刷新成功</returns>
    [HttpGet("Refresh")]
    [ApiExplorerSettings(GroupName = "v5")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ProducesResponseType(200, Type = typeof(object))]
    public async Task<IActionResult> Refresh([FromQuery(Name = "uid")] string? uid)
    {
        if (!int.TryParse(uid, out _) || uid.Length != 9)
        {
            return this.Fail($"{uid}不是合法的uid");
        }

        return this.Success("角色展柜数据刷新成功");
    }

    /// <summary>
    /// 刷新角色展柜数据
    /// </summary>
    /// <returns>是否刷新成功</returns>
    [HttpGet("Avatars")]
    [ApiExplorerSettings(GroupName = "v5")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ProducesResponseType(200, Type = typeof(object))]
    public async Task<IActionResult> GetAvatars()
    {
        return this.Success("test");
    }

    /// <summary>
    /// 刷新角色展柜数据
    /// </summary>
    /// <returns>是否刷新成功</returns>
    [HttpGet("Details")]
    [ApiExplorerSettings(GroupName = "v5")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ProducesResponseType(200, Type = typeof(object))]
    public async Task<IActionResult> GetAvatarsDetail()
    {
        return this.Success("test");
    }
}