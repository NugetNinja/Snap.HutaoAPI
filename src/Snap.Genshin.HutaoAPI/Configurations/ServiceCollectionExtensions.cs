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

        public static IServiceCollection AddUserSecretManager(this IServiceCollection services, Action<SecretManagerConfiguration> options)
        {
            SecretManagerConfiguration config = new();
            options.Invoke(config);

            Must.NotNull(config.SymmetricKey);
            Must.NotNull(config.HashSalt);
            Must.NotNull(config.SymmetricSalt);

            return services.AddTransient<ISecretManager, UserSecretManager>(services =>
                new UserSecretManager(
                    services.GetRequiredService<ApplicationDbContext>(),
                    services.GetRequiredService<ILogger<UserSecretManager>>(),
                    config));
        }
    }
}
