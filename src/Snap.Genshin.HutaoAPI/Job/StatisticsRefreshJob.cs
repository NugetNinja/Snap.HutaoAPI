using Quartz;
using Snap.HutaoAPI.Services;

namespace Snap.HutaoAPI.Job;

/// <summary>
/// 刷新统计数据任务
/// </summary>
public class StatisticsRefreshJob : IJob
{
    private readonly ILogger<StatisticsRefreshJob> logger;
    private readonly GenshinStatisticsService statisticsService;

    /// <summary>
    /// 构造一个新的刷新统计数据任务
    /// </summary>
    /// <param name="logger">日志器</param>
    /// <param name="statisticsService">统计服务</param>
    public StatisticsRefreshJob(ILogger<StatisticsRefreshJob> logger, GenshinStatisticsService statisticsService)
    {
        this.logger = logger;
        this.statisticsService = statisticsService;
    }

    /// <inheritdoc/>
    public async Task Execute(IJobExecutionContext context)
    {
        logger.LogInformation("[{time}] 已触发数据更新...", DateTime.Now);

        // 计算数据
        await statisticsService
            .CalculateAllStatistics()
            .ConfigureAwait(false);
    }
}
