// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Mvc;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Models;
using Snap.HutaoAPI.Services;

namespace Snap.HutaoAPI.Controllers;

/// <summary>
/// 测试控制器
/// </summary>
[Route("[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> logger;
    private readonly GenshinStatisticsService statisticsService;
    private readonly ApplicationDbContext dbContext;

    /// <summary>
    /// 测试控制器
    /// </summary>
    /// <param name="logger">日志器</param>
    /// <param name="dbContext">数据库上下文</param>
    /// <param name="statisticsService">统计服务</param>
    public TestController(
        ILogger<TestController> logger,
        ApplicationDbContext dbContext,
        GenshinStatisticsService statisticsService)
    {
        this.logger = logger;
        this.statisticsService = statisticsService;
        this.dbContext = dbContext;
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    /// <returns>任务</returns>
    [HttpGet("[Action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> RefreshStatistics()
    {
        logger.LogInformation("已触发统计数据更新...");

        // 计算数据
        await statisticsService
            .CalculateAllStatistics()
            .ConfigureAwait(false);

        return this.Success("完成");
    }

    /// <summary>
    /// 删除当期统计数据
    /// </summary>
    /// <returns>任务</returns>
    [HttpGet("[Action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> ClearStatistics()
    {
        logger.LogInformation("已触发数据清理...");

        // TODO 旧记录存档
        dbContext.PlayerRecords.RemoveRange(dbContext.PlayerRecords);

        await dbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return this.Success("完成");
    }
}
