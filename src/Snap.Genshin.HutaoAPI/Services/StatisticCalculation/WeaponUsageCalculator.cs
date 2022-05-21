using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Entities.Record;
using Snap.HutaoAPI.Models.Statistics;

namespace Snap.HutaoAPI.Services.StatisticCalculation
{
    [Obsolete("Should not use StatisticCalculation anymore")]
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
            // TODO AsEnumerable 可能带来性能问题
            // TODO 应该跳过玩家未上场的角色
            IEnumerable<IGrouping<int, AvatarDetail>> avatarGroups = dbContext.AvatarDetails
                .AsEnumerable()
                .GroupBy(avatar => avatar.AvatarId);

            List<WeaponUsage> result = new(avatarGroups.Count());

            foreach (IGrouping<int, AvatarDetail>? avatarGroup in avatarGroups)
            {
                if (!dbContext.SpiralAbyssAvatars.Any(avatar => avatar.AvatarId == avatarGroup.Key))
                {
                    continue;
                }

                int count = avatarGroup.Count();
                List<Rate<int>> weaponRateList = new(32);
                WeaponUsage avatarWeaponUsage = new() { Avatar = avatarGroup.Key, Weapons = weaponRateList };

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
