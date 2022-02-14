using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Entities.Record;
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
            // TODO AsEnumerable可能带来性能问题
            IEnumerable<IGrouping<int, AvatarDetail>> avatarGroups = dbContext.AvatarDetails
                .AsEnumerable()
                .GroupBy(avatar => avatar.AvatarId);

            List<WeaponUsage> result = new(avatarGroups.Count());

            foreach (IGrouping<int, AvatarDetail>? avatarGroup in avatarGroups)
            {
                //TODO 应该跳过某名玩家未上场的角色
                if (!dbContext.SpiralAbyssAvatars.Any(avatar => avatar.AvatarId == avatarGroup.Key))
                {
                    continue;
                }

                List<Rate<int>> weaponRateList = new(32);
                WeaponUsage avatarWeaponUsage = new() { Avatar = avatarGroup.Key, Weapons = weaponRateList };
                int count = avatarGroup.Count();
                IEnumerable<IGrouping<int, AvatarDetail>> weaponGroup = avatarGroup
                    .AsEnumerable()
                    .GroupBy(avatar => avatar.WeaponId)
                    .OrderByDescending(group => group.Count())
                    .Take(8);
                // 取武器使用率前8
                foreach (IGrouping<int, AvatarDetail>? weapon in weaponGroup)
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
