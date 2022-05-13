using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snap.HutaoAPI.Models.Utility;
using Snap.HutaoAPI.Services;

#if DEBUG
namespace Snap.HutaoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TestController : ControllerBase
    {
        public TestController(IServiceProvider serviceProvider, ILogger logger)
        {
            this.serviceProvider = serviceProvider;
            this.logger = logger;
        }

        private readonly IServiceProvider serviceProvider;
        private readonly ILogger logger;

        [HttpGet("RefreshStatistics")]
        public async Task<IActionResult> RefreshStatistics()
        {
            logger.LogWarning("数据刷新被手动触发");
            await serviceProvider
                .GetRequiredService<GenshinStatisticsService>()
                .CaltulateStatistics();
            return Ok();
        }

        [HttpGet("[Action]")]
        [Authorize(IdentityPolicyNames.CommonUser)]
        public IActionResult AuthTest()
        {
            return Ok();
        }
    }
}
#endif