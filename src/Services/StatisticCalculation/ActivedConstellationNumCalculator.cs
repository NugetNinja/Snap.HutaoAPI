using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Models.Statistics;

namespace Snap.Genshin.Website.Services.StatisticCalculation
{
    public class ActivedConstellationNumCalculator : IStatisticCalculator
    {
        public ActivedConstellationNumCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        {
            this.dbContext = dbContext;
            this.statisticsProvider = statisticsProvider;
        }

        private readonly ApplicationDbContext dbContext;
        private readonly IStatisticsProvider statisticsProvider;

        public async Task Calculate()
        {
            var avatarGroup = dbContext.AvatarDetails.AsEnumerable().GroupBy(avatar => avatar.AvatarId);

            var result = new List<AvatarConstellationNum>(64);

            foreach (IGrouping<int, Entities.Record.AvatarDetail>? group in avatarGroup)
            {
                // 跳过从未出现在深渊中的角色
                // if (!dbContext.SpiralAbyssAvatars.Any(avatar => avatar.AvatarId == group.Key)) continue;

                var countDic = new Dictionary<int, int>
                {
                    {0, 0},{1, 0},{2, 0},{3, 0},{4, 0},{5, 0},{6, 0}
                };

                foreach(var avatar in group)
                {
                    countDic[avatar.ActivedConstellationNum]++;
                }
                var rate = from kv in countDic select new Rate<int> { Id = kv.Key, Value = (double)kv.Value / @group.Count() };
                result.Add(new AvatarConstellationNum { Avatar = group.Key, Rate = rate });
            }

            await statisticsProvider.SaveStatistics<ActivedConstellationNumCalculator>(result);
        }
    }
}
