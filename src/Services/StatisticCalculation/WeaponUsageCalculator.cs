using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Models.Statistics;

namespace Snap.Genshin.Website.Services.StatisticCalculation
{
    public class WeaponUsageCalculator : IStatisticCalculator
    {
        public WeaponUsageCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        {
            this.dbContext = dbContext;
            this.statisticsProvider = statisticsProvider;
        }

        private readonly ApplicationDbContext dbContext;
        private readonly IStatisticsProvider statisticsProvider;

        public async Task Calculate()
        {
            var avatarGroup = dbContext.AvatarDetails.GroupBy(avatar => avatar.AvatarId);

            var result = new List<WeaponUsage>(avatarGroup.Count());

            foreach (var group in avatarGroup)
            {
                var weaponRateList = new List<Rate<int>>(32);
                var avatarWeaponUsage = new WeaponUsage { Avatar = group.Key, Weapons = weaponRateList };
                var count = avatarGroup.Count();
                var weaponGroup = group.AsEnumerable().GroupBy(avatar => avatar.WeaponId);
                // 取武器使用率前8
                foreach (var weapon in weaponGroup.Take(8))
                {
                    weaponRateList.Add(new()
                    {
                        Id = weapon.Key,
                        Value = (double)weapon.Count() / count
                    });
                }
                result.Add(avatarWeaponUsage);
            }

            await statisticsProvider.SaveStatistics<WeaponUsageCalculator>(result);
        }
    }
}
