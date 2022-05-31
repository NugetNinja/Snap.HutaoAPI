using Microsoft.AspNetCore.Mvc;
using Snap.HutaoAPI.Models;
using Snap.HutaoAPI.Services;

namespace Snap.HutaoAPI.Controllers;

[Route("[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> logger;
    private readonly GenshinStatisticsService statisticsService;

    /// <summary>
    /// 测试控制器
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="statisticsService"></param>
    public TestController(ILogger<TestController> logger, GenshinStatisticsService statisticsService)
    {
        this.logger = logger;
        this.statisticsService = statisticsService;
    }

    [HttpGet("[Action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Run()
    {
        logger.LogInformation("已触发测试数据更新...");

        // 计算数据
        await statisticsService.CalculateAllStatistics().ConfigureAwait(false);

        return this.Success("完成");
    }
}
