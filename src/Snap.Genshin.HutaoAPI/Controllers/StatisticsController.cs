// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snap.HutaoAPI.Models;
using Snap.HutaoAPI.Models.Identity;
using Snap.HutaoAPI.Models.Statistics;
using Snap.HutaoAPI.Services.Abstraction;
using Snap.HutaoAPI.Services.ParallelCalculation;
using Snap.HutaoAPI.Services.StatisticCalculation;

namespace Snap.HutaoAPI.Controllers;

/// <summary>
/// 统计数据控制器
/// </summary>
[Route("[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly IStatisticsProvider statisticsProvider;

    /// <summary>
    /// 构造一个新的统计数据控制器
    /// </summary>
    /// <param name="statisticsProvider">统计提供器</param>
    public StatisticsController(IStatisticsProvider statisticsProvider)
    {
        this.statisticsProvider = statisticsProvider;
    }

    /// <summary>
    /// 获取总览数据
    /// </summary>
    /// <returns>总览数据</returns>
    [HttpGet("Overview")]
    [ApiExplorerSettings(GroupName = "v2")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ProducesResponseType(200, Type = typeof(ApiResponse<OverviewData>))]
    public async Task<IActionResult> GetOverviewData()
    {
        OverviewData? result = await statisticsProvider
            .ReadStatisticAsync<OverviewDataCalculator, OverviewData>()
            .ConfigureAwait(false);

        return result is null
            ? this.Fail(ApiCode.ServiceConflict, "服务冲突")
            : this.Success("总览数据获取成功", result);
    }

    /// <summary>
    /// 获取角色出场数据
    /// </summary>
    /// <returns>角色出场数据</returns>
    [HttpGet("AvatarParticipation")]
    [ApiExplorerSettings(GroupName = "v2")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<AvatarParticipation>>))]
    public async Task<IActionResult> GetAvatarParticipation()
    {
        IEnumerable<AvatarParticipation>? result = await statisticsProvider
            .ReadStatisticAsync<AvatarParticipationCalculator, IEnumerable<AvatarParticipation>>()
            .ConfigureAwait(false);

        return result is null
            ? this.Fail(ApiCode.ServiceConflict, "服务冲突")
            : this.Success("出场率数据获取成功", result);
    }

    /// <summary>
    /// 获取圣遗物数据
    /// </summary>
    /// <returns>圣遗物数据</returns>
    [HttpGet("AvatarReliquaryUsage")]
    [ApiExplorerSettings(GroupName = "v2")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<AvatarReliquaryUsage>>))]
    public async Task<IActionResult> GetAvatarReliquaryUsage()
    {
        IEnumerable<AvatarReliquaryUsage>? result = await statisticsProvider
            .ReadStatisticAsync<AvatarReliquaryUsageCalculator, IEnumerable<AvatarReliquaryUsage>>()
            .ConfigureAwait(false);

        return result is null
            ? this.Fail(ApiCode.ServiceConflict, "服务冲突")
            : this.Success("圣遗物数据获取成功", result);
    }

    /// <summary>
    /// 获取角色搭配数据
    /// </summary>
    /// <returns>角色搭配数据</returns>
    [HttpGet("TeamCollocation")]
    [ApiExplorerSettings(GroupName = "v2")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<TeamCollocation>>))]
    public async Task<IActionResult> GetTeamCollocation()
    {
        IEnumerable<TeamCollocation>? result = await statisticsProvider
            .ReadStatisticAsync<TeamCollocationCalculator, IEnumerable<TeamCollocation>>()
            .ConfigureAwait(false);

        return result is null
            ? this.Fail(ApiCode.ServiceConflict, "服务冲突")
            : this.Success("角色搭配数据获取成功", result);
    }

    /// <summary>
    /// 获取武器使用数据
    /// </summary>
    /// <returns>武器使用数据</returns>
    [HttpGet("AvatarWeaponUsage")]
    [ApiExplorerSettings(GroupName = "v2")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<WeaponUsage>>))]
    public async Task<IActionResult> GetAvatarWeaponUsage()
    {
        IEnumerable<WeaponUsage>? result = await statisticsProvider
            .ReadStatisticAsync<WeaponUsageCalculator, IEnumerable<WeaponUsage>>()
            .ConfigureAwait(false);

        return result is null
            ? this.Fail(ApiCode.ServiceConflict, "服务冲突")
            : this.Success("武器数据获取成功", result);
    }

    /// <summary>
    /// 获取命座数据
    /// </summary>
    /// <returns>命座数据</returns>
    [HttpGet("Constellation")]
    [ApiExplorerSettings(GroupName = "v2")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<AvatarConstellationInfo>>))]
    public async Task<IActionResult> GetConstellation()
    {
        IEnumerable<AvatarConstellationInfo>? result = await statisticsProvider
            .ReadStatisticAsync<ActivedConstellationNumCalculator, IEnumerable<AvatarConstellationInfo>>()
            .ConfigureAwait(false);

        return result is null
            ? this.Fail(ApiCode.ServiceConflict, "服务冲突")
            : this.Success("命座数据获取成功", result);
    }

    /// <summary>
    /// 获取队伍出场数据
    /// </summary>
    /// <returns>队伍出场数据</returns>
    [HttpGet("TeamCombination")]
    [ApiExplorerSettings(GroupName = "v2")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<LevelTeamUsage>>))]
    public async Task<IActionResult> GetTeamCombination()
    {
        IEnumerable<LevelTeamUsage>? result = await statisticsProvider
            .ReadStatisticAsync<TeamCombinationCalculator, IEnumerable<LevelTeamUsage>>()
            .ConfigureAwait(false);

        return result is null
            ? this.Fail(ApiCode.ServiceConflict, "服务冲突")
            : this.Success("队伍使用数据获取成功", result.Select(usage => usage.ReduceTeamsTo(24)));
    }
}
