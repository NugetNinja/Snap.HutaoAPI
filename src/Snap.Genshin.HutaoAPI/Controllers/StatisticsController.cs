// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snap.Genshin.Website.Models;
using Snap.Genshin.Website.Models.Statistics;
using Snap.Genshin.Website.Models.Utility;
using Snap.Genshin.Website.Services;
using Snap.Genshin.Website.Services.StatisticCalculation;
using System.Text.Json;

namespace Snap.Genshin.Website.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        public StatisticsController(ILogger<StatisticsController> logger, IStatisticsProvider statisticsProvider)
        {
            this.logger = logger;
            this.statisticsProvider = statisticsProvider;
        }

        private readonly IStatisticsProvider statisticsProvider;
        private readonly ILogger logger;

        /// <summary>
        /// 获取角色出场数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("AvatarParticipation")]
        [ApiExplorerSettings(GroupName = "v2")]
        [Authorize(IdentityPolicyNames.CommonUser)]
        [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<AvatarParticipation>>))]
        public async Task<IActionResult> GetAvatarParticipation()
        {
            string? json = await statisticsProvider
                .ReadStatistics<AvatarParticipationCalculator>()
                .ConfigureAwait(false);
            return json is null
                ? this.Fail(ApiCode.ServiceConcurrent, "服务冲突")
                : this.Success("出场率数据获取成功", JsonSerializer.Deserialize<IEnumerable<AvatarParticipation>>(json));
        }

        /// <summary>
        /// 获取总览数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("Overview")]
        [ApiExplorerSettings(GroupName = "v2")]
        [Authorize(IdentityPolicyNames.CommonUser)]
        [ProducesResponseType(200, Type = typeof(ApiResponse<OverviewData>))]
        public async Task<IActionResult> GetOverviewData()
        {
            string? json = await statisticsProvider
                .ReadStatistics<OverviewDataCalculator>()
                .ConfigureAwait(false);
            return json is null
                ? this.Fail(ApiCode.ServiceConcurrent, "服务冲突")
                : this.Success("总览数据获取成功", JsonSerializer.Deserialize<OverviewData>(json));
        }

        /// <summary>
        /// 获取圣遗物数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("AvatarReliquaryUsage")]
        [ApiExplorerSettings(GroupName = "v2")]
        [Authorize(IdentityPolicyNames.CommonUser)]
        [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<AvatarReliquaryUsage>>))]
        public async Task<IActionResult> GetAvatarReliquaryUsage()
        {
            string? json = await statisticsProvider
                .ReadStatistics<AvatarReliquaryUsageCalculator>()
                .ConfigureAwait(false);
            return json is null
                ? this.Fail(ApiCode.ServiceConcurrent, "服务冲突")
                : this.Success("圣遗物数据获取成功", JsonSerializer.Deserialize<IEnumerable<AvatarReliquaryUsage>>(json));
        }

        /// <summary>
        /// 获取组队数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("TeamCollocation")]
        [ApiExplorerSettings(GroupName = "v2")]
        [Authorize(IdentityPolicyNames.CommonUser)]
        [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<TeamCollocation>>))]
        public async Task<IActionResult> GetTeamCollocation()
        {
            string? json = await statisticsProvider
                .ReadStatistics<TeamCollocationCalculator>()
                .ConfigureAwait(false);
            return json is null
                ? this.Fail(ApiCode.ServiceConcurrent, "服务冲突")
                : this.Success("组队数据获取成功", JsonSerializer.Deserialize<IEnumerable<TeamCollocation>>(json));
        }

        /// <summary>
        /// 获取武器数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("WeaponUsage")]
        [ApiExplorerSettings(GroupName = "v2")]
        [Authorize(IdentityPolicyNames.CommonUser)]
        [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<WeaponUsage>>))]
        public async Task<IActionResult> GetWeaponUsage()
        {
            string? json = await statisticsProvider
                .ReadStatistics<WeaponUsageCalculator>()
                .ConfigureAwait(false);
            return json is null
                ? this.Fail(ApiCode.ServiceConcurrent, "服务冲突")
                : this.Success("武器数据获取成功", JsonSerializer.Deserialize<IEnumerable<WeaponUsage>>(json));
        }

        /// <summary>
        /// 获取命座数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("Constellation")]
        [ApiExplorerSettings(GroupName = "v2")]
        [Authorize(IdentityPolicyNames.CommonUser)]
        [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<AvatarConstellationNum>>))]
        public async Task<IActionResult> GetConstellation()
        {
            string? json = await statisticsProvider
                .ReadStatistics<ActivedConstellationNumCalculator>()
                .ConfigureAwait(false);
            return json is null
                ? this.Fail(ApiCode.ServiceConcurrent, "服务冲突")
                : this.Success("命座数据获取成功", JsonSerializer.Deserialize<IEnumerable<AvatarConstellationNum>>(json));
        }

        /// <summary>
        /// 获取队伍使用数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("TeamCombination")]
        [ApiExplorerSettings(GroupName = "v2")]
        [Authorize(IdentityPolicyNames.CommonUser)]
        [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<LevelTeamUsage>>))]
        public async Task<IActionResult> TeamCombination()
        {
            string? json = await statisticsProvider
                .ReadStatistics<TeamCombinationCalculator>()
                .ConfigureAwait(false);
            return json is null
                ? this.Fail(ApiCode.ServiceConcurrent, "服务冲突")
                : this.Success("队伍使用数据获取成功", JsonSerializer.Deserialize<IEnumerable<LevelTeamUsage>>(json));
        }
    }
}
