using Microsoft.AspNetCore.Mvc;
using Snap.Genshin.Website.Services;
#if DEBUG
namespace Snap.Genshin.Website.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        public TestController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        private readonly IServiceProvider serviceProvider;

        [HttpGet("RefreshStatistics")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> RefreshStatistics()
        {
            await serviceProvider
                .GetRequiredService<GenshinStatisticsService>()
                .CaltulateStatistics();
            return Ok();
        }
    }
}
#endif