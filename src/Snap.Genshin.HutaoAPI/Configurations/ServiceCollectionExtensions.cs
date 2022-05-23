// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.HutaoAPI.Services;

namespace Snap.HutaoAPI.Configurations;

/// <summary>
/// 服务集合扩展
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 添加统计服务
    /// </summary>
    /// <param name="services">操作的服务集合</param>
    /// <param name="options">操作</param>
    /// <returns>可继续操作的服务集合</returns>
    public static IServiceCollection AddGenshinStatistics(this IServiceCollection services, Action<GenshinStatisticsServiceConfiguration> options)
    {
        GenshinStatisticsServiceConfiguration config = new();
        options.Invoke(config);

        config.CalculatorTypes.ForEach(type => services.AddScoped(type));

        services.AddTransient(services =>
        {
            // IServiceScope scope = services.CreateScope();
            // ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            ILogger<GenshinStatisticsService> logger = services.GetRequiredService<ILogger<GenshinStatisticsService>>();
            IServiceProvider serviceProvider = services.GetRequiredService<IServiceProvider>();

            return new GenshinStatisticsService(config, logger, serviceProvider);
        });

        return services;
    }
}
