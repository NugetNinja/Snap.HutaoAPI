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
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<AvatarParticipation>>))]
        public async Task<IActionResult> GetAvatarParticipation()
        {
            string? json = await statisticsProvider.ReadStatistics<AvatorParticipationCalculator>()
                                               .ConfigureAwait(false);
            if (json is null)
            {
                return this.Fail(ApiCode.ServiceConcurrent, "服务冲突");
            }

            return this.Success("出场率数据获取成功", JsonSerializer.Deserialize<IEnumerable<AvatarParticipation>>(json));
        }

        /// <summary>
        /// 获取总览数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("Overview")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        [ProducesResponseType(200, Type = typeof(ApiResponse<OverviewData>))]
        public async Task<IActionResult> GetOverviewData()
        {
            string? json = await statisticsProvider.ReadStatistics<OverviewDataCalculator>()
                                               .ConfigureAwait(false);
            if (json is null)
            {
                return this.Fail(ApiCode.ServiceConcurrent, "服务冲突");
            }

            return this.Success("总览数据获取成功", JsonSerializer.Deserialize<OverviewData>(json));
        }

        /// <summary>
        /// 获取圣遗物数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("AvatarReliquaryUsage")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<AvatarReliquaryUsage>>))]
        public async Task<IActionResult> GetAvatarReliquaryUsage()
        {
            string? json = await statisticsProvider.ReadStatistics<AvatarReliquaryUsageCalculator>()
                                               .ConfigureAwait(false);
            if (json is null)
            {
                return this.Fail(ApiCode.ServiceConcurrent, "服务冲突");
            }

            return this.Success("圣遗物数据获取成功", JsonSerializer.Deserialize<IEnumerable<AvatarReliquaryUsage>>(json));
        }

        /// <summary>
        /// 获取组队数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("TeamCollocation")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<TeamCollocation>>))]
        public async Task<IActionResult> GetTeamCollocation()
        {
            string? json = await statisticsProvider.ReadStatistics<TeamCollocationCalculator>()
                                               .ConfigureAwait(false);
            if (json is null)
            {
                return this.Fail(ApiCode.ServiceConcurrent, "服务冲突");
            }

            return this.Success("组队数据获取成功", JsonSerializer.Deserialize<IEnumerable<TeamCollocation>>(json));
        }

        /// <summary>
        /// 获取武器数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("WeaponUsage")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        [ProducesResponseType(200, Type = typeof(ApiResponse<IEnumerable<WeaponUsage>>))]
        public async Task<IActionResult> GetWeaponUsage()
        {
            string? json = await statisticsProvider.ReadStatistics<WeaponUsageCalculator>()
                                               .ConfigureAwait(false);
            if (json is null)
            {
                return this.Fail(ApiCode.ServiceConcurrent, "服务冲突");
            }

            return this.Success("武器数据获取成功", JsonSerializer.Deserialize<IEnumerable<WeaponUsage>>(json));
        }
    }
}
