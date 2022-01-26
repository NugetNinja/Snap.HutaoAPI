using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Services;

namespace Snap.Genshin.Website.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGenshinStatisticsService(this IServiceCollection services,
            Action<GenshinStatisticsServiceConfiguration> options)
        {
            GenshinStatisticsServiceConfiguration? config = new GenshinStatisticsServiceConfiguration();
            options.Invoke(config);

            services.AddTransient(services =>
            {
                ILogger<GenshinStatisticsService>? logger = services.GetRequiredService<ILogger<GenshinStatisticsService>>();
                ApplicationDbContext? dbContext = services.GetRequiredService<ApplicationDbContext>();
                return new GenshinStatisticsService(config, logger, dbContext);
            });

            return services;
        }
    }
}
