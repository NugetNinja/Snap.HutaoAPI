// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Snap.Genshin.MapReduce;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Models.Statistics;
using System.Collections.Concurrent;

namespace Snap.HutaoAPI.Services.MapReduceCalculation
{
    /// <summary>
    /// 命座持有率计算器
    /// </summary>
    public class ActivedConstellationNumCalculator : IStatisticCalculator
    {
        /// <summary>
        /// 构造一个新的命座持有率计算器
        /// </summary>
        /// <param name="dbContext">数据库上下文</param>
        /// <param name="statisticsProvider">统计提供器</param>
        public ActivedConstellationNumCalculator(ApplicationDbContext dbContext, IStatisticsProvider statisticsProvider)
        {
            this.dbContext = dbContext;
            this.statisticsProvider = statisticsProvider;
        }

        private readonly ApplicationDbContext dbContext;
        private readonly IStatisticsProvider statisticsProvider;

        /// <inheritdoc/>
        public async Task Calculate()
        {
            int totalPlayerCount = this.dbContext.Players.Count();
            IQueryable<AvatarWithConstellationNum> avatars = (from avatar in dbContext.AvatarDetails
                select new AvatarWithConstellationNum(avatar.AvatarId, avatar.ActivedConstellationNum))
                .AsNoTracking();

            Reducer<AvatarWithConstellationNum, int, ConcurrentBag<int>> groupReducer = new((input, result) =>
            {
                result
                    .GetOrAdd(input.AvatarId, (_) => new ConcurrentBag<int>())
                    .Add(input.ActivedNum);
            });

            groupReducer.Reduce(avatars);

            ConcurrentBag<AvatarConstellationNum> calculationResult = new();

            Parallel.ForEach(groupReducer.ReduceResult, kv =>
            {
                int currentAvatarCount = 0;

                Reducer<int, int, int> reducer = new((input, result) =>
                {
                    result.AddOrUpdate(input, 1, (key, oldValue) => Interlocked.Increment(ref oldValue));
                    Interlocked.Increment(ref currentAvatarCount);
                });

                reducer.Reduce(kv.Value);

                IEnumerable<Rate<int>> rate = from kvp in reducer.ReduceResult
                    select new Rate<int>
                    {
                        Id = kvp.Key,
                        Value = (double)kvp.Value / currentAvatarCount,
                    };

                calculationResult.Add(new()
                {
                    Avatar = kv.Key, Rate = rate,
                    HoldingRate = (double)kv.Value.Count / totalPlayerCount,
                });
            });

            await statisticsProvider.SaveStatistics<ActivedConstellationNumCalculator>(calculationResult);
        }
    }

    internal record AvatarWithConstellationNum(int AvatarId, int ActivedNum);
}
