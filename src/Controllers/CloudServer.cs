using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Services;

namespace Snap.Genshin.Website.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CloudServer : ControllerBase
    {
        public CloudServer(ILogger<CloudServer> logger, IConfiguration configuration, 
            GenshinStatisticsService statisticsService, ApplicationDbContext dbContext,
            IMemoryCache cache)
        {
            this.logger = logger;
            this.cloudToken = configuration.GetSection("TencentCloud")
                                           .GetValue<string>("CloudToken");
            this.statisticsService = statisticsService;
            this.dbContext = dbContext;
            this.cache = cache;
        }

        private readonly ILogger logger;
        private readonly string cloudToken;
        private readonly GenshinStatisticsService statisticsService;
        private readonly ApplicationDbContext dbContext;
        private readonly IMemoryCache cache;

        [HttpGet("[Action]")]
        public async Task<IActionResult> CalculateStatistics()
        {
            if (Request.Headers["CloudToken"] != this.cloudToken) return Unauthorized();

            this.logger.LogInformation("已触发数据更新...");

            // 写入忙碌标识
            cache.Set("_STATISTICS_BUSY", true);

            // 计算数据
            await this.statisticsService.CaltulateStatistics().ConfigureAwait(false);

            // 清除忙碌标识
            cache.Remove("_STATISTICS_BUSY");
            
            return Ok();
        }

        [HttpGet("[Action]")]
        public async Task<IActionResult> ClearStatistics()
        {
            if (Request.Headers["CloudToken"] != this.cloudToken) return Unauthorized();

            this.logger.LogInformation("已触发数据清理...");
            
            // TODO 旧记录存档
            dbContext.PlayerRecords.RemoveRange(dbContext.PlayerRecords);

            await dbContext.SaveChangesAsync().ConfigureAwait(false);

            return Ok();
        }
    }

}
