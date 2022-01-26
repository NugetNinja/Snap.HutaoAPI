using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Models.Statistics;
using System.Text.Json;

namespace Snap.Genshin.Website.Services.StatisticCalculation
{
    public class WeaponUsageCalculator : IStatisticCalculator
    {
        public WeaponUsageCalculator(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private readonly ApplicationDbContext dbContext;

        public async Task Calculate()
        {
            var avatarGroup = dbContext.AvatarDetails.GroupBy(avatar => avatar.AvatarId);

            var result = new List<WeaponUsage>(avatarGroup.Count());

            foreach(var group in avatarGroup)
            {
                var weaponRateList = new List<Rate<int>>(32);
                var avatarWeaponUsage = new WeaponUsage { Avatar = group.Key, Weapons = weaponRateList };
                var count = avatarGroup.Count();
                var weaponGroup = group.AsEnumerable().GroupBy(avatar => avatar.WeaponId);
                // 取武器使用率前8
                foreach(var weapon in weaponGroup.Take(8))
                {
                    weaponRateList.Add(new()
                    {
                        Id = weapon.Key,
                        Value = (double)weapon.Count() / count
                    });
                }
                result.Add(avatarWeaponUsage);
            }

            // 新增或修改当期数据
            var periodId = IStatisticCalculator.GetSpiralPeriodId(DateTime.Now);
            var data = dbContext.Statistics.Where(s => s.Source == nameof(WeaponUsageCalculator))
                                           .Where(s => s.Period == periodId)
                                           .SingleOrDefault();
            if (data is null)
            {
                data = new();
                dbContext.Statistics.Add(data);
            }
            data.Period = periodId;
            data.Source = nameof(WeaponUsageCalculator);
            data.Value = JsonSerializer.Serialize(result);

            await dbContext.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
