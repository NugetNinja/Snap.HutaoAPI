using Microsoft.EntityFrameworkCore;
using Snap.Genshin.MapReduce;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Models.Statistics;
using Snap.Genshin.Website.Services.StatisticCalculation;
using System.Collections.Concurrent;

namespace Snap.Genshin.Website.Services.MapReduceCalculation
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
            int totalPlayerCount = dbContext.Players.Count();
            IQueryable<AvatarWithConstellationNum>? avatars = (from avatar in dbContext.AvatarDetails select new AvatarWithConstellationNum(avatar.AvatarId, avatar.ActivedConstellationNum)).AsNoTracking();

            Reducer<AvatarWithConstellationNum, int, ConcurrentBag<int>>? groupReducer = new Reducer<AvatarWithConstellationNum, int, ConcurrentBag<int>>((input, result) =>
            {
                result.GetOrAdd(input.AvatarId, new ConcurrentBag<int>())
                      .Add(input.ActivedNum);
            });

            groupReducer.Reduce(avatars);

            ConcurrentBag<AvatarConstellationNum>? calculationResult = new ConcurrentBag<AvatarConstellationNum>();

            Parallel.ForEach(groupReducer.ReduceResult, kv =>
            {
                int currentAvatarCount = 0;

                Reducer<int, int, int>? reducer = new Reducer<int, int, int>((input, result) =>
                {
                    result.AddOrUpdate(input, 1, (key, oldValue) => Interlocked.Increment(ref oldValue));
                    Interlocked.Increment(ref currentAvatarCount);
                });

                reducer.Reduce(kv.Value);

                IEnumerable<Rate<int>>? rate = from kvp in reducer.ReduceResult select new Rate<int> { Id = kvp.Key, Value = (double)kvp.Value / currentAvatarCount };
                calculationResult.Add(new() { Avatar = kv.Key, Rate = rate, HoldingRate = (double)kv.Value.Count / totalPlayerCount });
            });

            await statisticsProvider.SaveStatistics<ActivedConstellationNumCalculator>(calculationResult);
        }
    }

    internal record AvatarWithConstellationNum(int AvatarId, int ActivedNum);
}
