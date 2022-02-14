using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Services;

namespace Snap.Genshin.Website.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGenshinStatisticsService(this IServiceCollection services, Action<GenshinStatisticsServiceConfiguration> options)
        {
            GenshinStatisticsServiceConfiguration config = new();
            options.Invoke(config);

            config.CalculatorTypes.ForEach(type => services.AddScoped(type));

            services.AddTransient(services =>
            {
                IServiceScope scope = services.CreateScope();
                ApplicationDbContext dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                ILogger<GenshinStatisticsService> logger = services.GetRequiredService<ILogger<GenshinStatisticsService>>();
                IServiceProvider serviceProvider = services.GetRequiredService<IServiceProvider>();

                return new GenshinStatisticsService(config, logger, dbContext, serviceProvider);
            });

            return services;
        }

        public static IServiceCollection AddTokenFactory(this IServiceCollection services, Action<TokenFactoryConfiguration> options)
        {
            TokenFactoryConfiguration config = new();
            options.Invoke(config);

            _ = config.Audience ?? throw new Exception(nameof(config.Audience));
            _ = config.Issuer ?? throw new Exception(nameof(config.Issuer));
            _ = config.SigningKey ?? throw new Exception(nameof(config.SigningKey));

            services.AddScoped<ITokenFactory, TokenFactory>(services =>
            {
                ApplicationDbContext dbContext = services.GetRequiredService<ApplicationDbContext>();

                return new TokenFactory(dbContext, config);
            });

            return services;
        }

        public static IServiceCollection AddUserSecretManager(this IServiceCollection services, Action<SecretManagerConfiguration> options)
        {
            SecretManagerConfiguration config = new();
            options.Invoke(config);

            _ = config.SymmetricKey ?? throw new Exception(nameof(config.SymmetricKey));
            _ = config.HashSalt ?? throw new Exception(nameof(config.HashSalt));
            _ = config.SymmetricSalt ?? throw new Exception(nameof(config.SymmetricSalt));

            services.AddTransient<ISecretManager, UserSecretManager>(services =>
            {
                ILogger<UserSecretManager> logger = services.GetRequiredService<ILogger<UserSecretManager>>();
                ApplicationDbContext dbContext = services.GetRequiredService<ApplicationDbContext>();

                return new UserSecretManager(dbContext, logger, config);
            });

            return services;
        }
    }
}
