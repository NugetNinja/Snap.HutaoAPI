// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snap.HutaoAPI.Models;
using Snap.HutaoAPI.Models.Identity;
using Snap.HutaoAPI.Models.Statistics;
using Snap.HutaoAPI.Services.Abstraction;
using Snap.HutaoAPI.Services.ParallelCalculation;

namespace Snap.HutaoAPI.Controllers;

/// <summary>
/// 统计数据控制器2
/// </summary>
[Route("[controller]")]
[ApiController]
public class Statistics2Controller : ControllerBase
{
    private readonly IStatisticsProvider statisticsProvider;

    /// <summary>
    /// 构造一个新的统计数据控制器
    /// </summary>
    /// <param name="statisticsProvider">统计提供器</param>
    public Statistics2Controller(IStatisticsProvider statisticsProvider)
    {
        this.statisticsProvider = statisticsProvider;
    }

    /// <summary>
    /// 获取队伍出场数据
    /// </summary>
    /// <returns>队伍出场数据</returns>
    [HttpGet("TeamCombination")]
    [ApiExplorerSettings(GroupName = "v4")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<FloorTeamUsage>>))]
    public async Task<IActionResult> GetTeamCombination()
    {
        IEnumerable<FloorTeamUsage>? result = await statisticsProvider
            .ReadStatisticAsync<TeamCombinationForFloorCalculator, IEnumerable<FloorTeamUsage>>()
            .ConfigureAwait(false);

        return result is null
            ? this.Fail(ApiCode.ServiceConflict, "服务冲突")
            : this.Success("队伍使用数据获取成功", result.Select(usage => usage.ReduceTeamsTo(16)));
    }

    /// <summary>
    /// 获取推荐的队伍
    /// </summary>
    /// <param name="desiredInfo">期望信息</param>
    /// <returns>队伍出场数据</returns>
    [HttpPost("TeamRecommanded")]
    [ApiExplorerSettings(GroupName = "v4")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ProducesResponseType(200, Type = typeof(ApiResponse<FloorTeamUsage>))]
    public async Task<IActionResult> GetRecommandTeams([FromBody] DesiredInfo desiredInfo)
    {
        IEnumerable<FloorTeamUsage>? result = await statisticsProvider
            .ReadStatisticAsync<TeamCombinationForFloorCalculator, IEnumerable<FloorTeamUsage>>()
            .ConfigureAwait(false);

        if (result is null)
        {
            return this.Fail(ApiCode.ServiceConflict, "服务冲突");
        }

        FloorTeamUsage? queriedFloor = result.SingleOrDefault(r => r.Floor == desiredInfo.Floor);

        if (queriedFloor is null)
        {
            return this.Fail("层数无效");
        }

        if (desiredInfo.DesiredAvatars is null || !desiredInfo.DesiredAvatars.Any())
        {
            return this.Fail("角色列表不应为空");
        }

        foreach (string avatar in desiredInfo.DesiredAvatars)
        {
            if (queriedFloor.Teams.Count() <= 4)
            {
                break;
            }

            queriedFloor.Teams = queriedFloor.Teams.Where(team => team.Id.Contains(avatar));
        }

        return this.Success("队伍使用数据获取成功", queriedFloor);
    }

    /// <summary>
    /// 封装期望楼层与角色列表
    /// </summary>
    public class DesiredInfo
    {
        /// <summary>
        /// 构造一个新的封装类
        /// </summary>
        /// <param name="floor">楼层</param>
        /// <param name="desiredAvatars">期望角色，按期望顺序排序</param>
        public DesiredInfo(int floor, IEnumerable<string> desiredAvatars)
        {
            Floor = floor;
            DesiredAvatars = desiredAvatars;
        }

        /// <summary>
        /// 层
        /// </summary>
        public int Floor { get; set; }

        /// <summary>
        /// 期望角色
        /// </summary>
        public IEnumerable<string> DesiredAvatars { get; set; }
    }
}