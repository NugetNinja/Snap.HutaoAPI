// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Models;
using Snap.HutaoAPI.Services;
using System.Diagnostics.CodeAnalysis;

namespace Snap.HutaoAPI.Controllers;

[SuppressMessage("", "SA1600")]
[Route("[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    private readonly ILogger<TestController> logger;
    private readonly ApplicationDbContext dbContext;
    private readonly GenshinStatisticsService statisticsService;

    public TestController(ILogger<TestController> logger, ApplicationDbContext dbContext, GenshinStatisticsService statisticsService)
    {
        this.logger = logger;
        this.dbContext = dbContext;
        this.statisticsService = statisticsService;
    }

    [HttpGet("[Action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Clear()
    {
        logger.LogInformation("[{time}] 已触发数据清理...", DateTime.Now);
        dbContext.Ranks.RemoveRange(dbContext.Ranks);

        DateTime now = DateTime.Now;
        TimeSpan threshold = TimeSpan.FromDays(45);
        DateTime lastAllowed = now - threshold;

        int count = dbContext.PlayerRecords
            .Include(record => record.Player)
            .Where(record => record.UploadTime < lastAllowed)
            .Select(record => record.Player)
            .Count();
        logger.LogInformation("{count} 个玩家应当移除...", count);

        dbContext.Players.RemoveRange(dbContext.PlayerRecords
            .Include(record => record.Player)
            .Where(record => record.UploadTime < lastAllowed)
            .Select(record => record.Player));

        await dbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);

        dbContext.PlayerRecords.RemoveRange(dbContext.PlayerRecords);
        await dbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);

        return this.Success("执行完成");
    }

    [HttpGet("[Action]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> Refresh()
    {
        logger.LogInformation("[{time}] 已触发数据更新...", DateTime.Now);

        // 计算数据
        await statisticsService
            .CalculateAllStatistics()
            .ConfigureAwait(false);

        return this.Success("执行完成");
    }
}