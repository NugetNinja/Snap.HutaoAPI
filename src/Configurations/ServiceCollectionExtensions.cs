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

        public static IServiceCollection AddTokenFactory(this IServiceCollection services, Action<TokenFactoryConfiguration> options)
        {
            var config = new TokenFactoryConfiguration();
            options(config);
            if (config.Audience is null) throw new Exception(nameof(config.Audience));
            if (config.Issuer is null) throw new Exception(nameof(config.Issuer));
            if (config.SigningKey is null) throw new Exception(nameof(config.SigningKey));

            services.AddScoped<ITokenFactory, TokenFactory>(services =>
            {
                var dbContext = services.GetRequiredService<ApplicationDbContext>();
                return new TokenFactory(dbContext, config);
            });

            return services;
        }
    }
}
