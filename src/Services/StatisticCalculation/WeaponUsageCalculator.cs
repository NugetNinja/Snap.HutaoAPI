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
            IQueryable<IGrouping<int, Entities.Record.AvatarDetail>>? avatarGroup = dbContext.AvatarDetails.GroupBy(avatar => avatar.AvatarId);

            List<WeaponUsage>? result = new List<WeaponUsage>(avatarGroup.Count());

            foreach (IGrouping<int, Entities.Record.AvatarDetail>? group in avatarGroup)
            {
                List<Rate<int>>? weaponRateList = new List<Rate<int>>(32);
                WeaponUsage? avatarWeaponUsage = new WeaponUsage { Avatar = group.Key, Weapons = weaponRateList };
                int count = avatarGroup.Count();
                IEnumerable<IGrouping<int, Entities.Record.AvatarDetail>>? weaponGroup = group.AsEnumerable().GroupBy(avatar => avatar.WeaponId);
                // 取武器使用率前8
                foreach (IGrouping<int, Entities.Record.AvatarDetail>? weapon in weaponGroup.Take(8))
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
