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

            foreach (var type in config.CalculatorTypes)
            {
                services.AddScoped(type);
            }

            services.AddScoped(services =>
            {
                ILogger<GenshinStatisticsService>? logger = services.GetRequiredService<ILogger<GenshinStatisticsService>>();
                ApplicationDbContext? dbContext = services.GetRequiredService<ApplicationDbContext>();
                IServiceProvider serviceProvider = services.GetRequiredService<IServiceProvider>();
                return new GenshinStatisticsService(config, logger, dbContext, serviceProvider);
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

        public static IServiceCollection AddUserSecretManager(this IServiceCollection services, Action<SecretManagerConfiguration> options)
        {
            var config = new SecretManagerConfiguration();
            options(config);

            if (config.SymmetricKey is null) throw new Exception(nameof(config.SymmetricKey));
            if (config.HashSalt is null) throw new Exception(nameof(config.HashSalt));
            if (config.SymmetricSalt is null) throw new Exception(nameof(config.SymmetricSalt));
            services.AddTransient<ISecretManager, UserSecretManager>(services =>
            {
                var logger = services.GetRequiredService<ILogger<UserSecretManager>>();
                var dbContext = services.GetRequiredService<ApplicationDbContext>();
                return new UserSecretManager(dbContext, logger, config);
            });

            return services;
        }
    }
}
