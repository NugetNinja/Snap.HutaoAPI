// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Quartz;
using Snap.HutaoAPI.Entities;

namespace Snap.HutaoAPI.Job;

/// <summary>
/// 刷新统计数据任务
/// </summary>
public class StatisticsClearJob : IJob
{
    private readonly ILogger<StatisticsClearJob> logger;
    private readonly ApplicationDbContext dbContext;

    /// <summary>
    /// 构造一个新的刷新统计数据任务
    /// </summary>
    /// <param name="logger">日志器</param>
    /// <param name="dbContext">数据库上下文</param>
    public StatisticsClearJob(ILogger<StatisticsClearJob> logger, ApplicationDbContext dbContext)
    {
        this.logger = logger;
        this.dbContext = dbContext;
    }

    /// <inheritdoc/>
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("[{time}] 已触发数据清理...", DateTime.Now);
        dbContext.Ranks.RemoveRange(dbContext.Ranks);

        DateTime now = DateTime.Now;
        TimeSpan threshold = TimeSpan.FromDays(45);

        dbContext.Players.RemoveRange(dbContext.PlayerRecords
            .Include(record => record.Player)
            .Where(record => now - record.UploadTime > threshold)
            .Select(record => record.Player));

        await dbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);

        dbContext.PlayerRecords.RemoveRange(dbContext.PlayerRecords);
        await dbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);
    }
}