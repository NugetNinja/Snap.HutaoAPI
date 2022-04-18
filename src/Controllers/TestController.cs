using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Snap.Genshin.Website.Models.Utility;
using Snap.Genshin.Website.Services;

#if DEBUG
namespace Snap.Genshin.Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TestController : ControllerBase
    {
        public TestController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        private readonly IServiceProvider serviceProvider;

        [HttpGet("RefreshStatistics")]
        public async Task<IActionResult> RefreshStatistics()
        {
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