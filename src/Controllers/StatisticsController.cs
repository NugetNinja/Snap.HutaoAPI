using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Snap.Genshin.Website.Models;
using Snap.Genshin.Website.Models.Utility;
using Snap.Genshin.Website.Services;
using Snap.Genshin.Website.Services.StatisticCalculation;

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
        public async Task<IActionResult> GetAvatarParticipation()
        {
            var data = await statisticsProvider.ReadStatistics<AvatorParticipationCaltulator>()
                                               .ConfigureAwait(false);
            if (data is null) return this.Fail("未找到该数据");

            return this.Success("出场率数据获取成功", data);
        }

        /// <summary>
        /// 获取总览数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("Overview")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> GetOverviewData()
        {
            var data = await statisticsProvider.ReadStatistics<OverviewDataCalculator>()
                                               .ConfigureAwait(false);
            if (data is null) return this.Fail("未找到该数据");

            return this.Success("出场率数据获取成功", data);
        }

        /// <summary>
        /// 获取圣遗物数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("AvatarReliquaryUsage")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> GetAvatarReliquaryUsage()
        {
            var data = await statisticsProvider.ReadStatistics<AvatarReliquaryUsageCalculator>()
                                               .ConfigureAwait(false);
            if (data is null) return this.Fail("未找到该数据");

            return this.Success("出场率数据获取成功", data);
        }

        /// <summary>
        /// 获取组队数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("TeamCollocation")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> GetTeamCollocation()
        {
            var data = await statisticsProvider.ReadStatistics<TeamCollocationCalculator>()
                                               .ConfigureAwait(false);
            if (data is null) return this.Fail("未找到该数据");

            return this.Success("出场率数据获取成功", data);
        }

        /// <summary>
        /// 获取武器数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("WeaponUsage")]
        [Authorize(Policy = IdentityPolicyNames.CommonUser)]
        public async Task<IActionResult> GetWeaponUsage()
        {
            var data = await statisticsProvider.ReadStatistics<WeaponUsageCalculator>()
                                               .ConfigureAwait(false);
            if (data is null) return this.Fail("未找到该数据");

            return this.Success("出场率数据获取成功", data);
        }
    }
}
