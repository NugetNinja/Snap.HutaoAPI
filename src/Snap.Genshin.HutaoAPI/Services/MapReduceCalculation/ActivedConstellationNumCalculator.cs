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
            int totalPlayerCount = dbContext.Players.Count();
            IQueryable<AvatarWithConstellationNum> avatars = (from avatar in dbContext.AvatarDetails
                select new AvatarWithConstellationNum(avatar.AvatarId, avatar.ActivedConstellationNum))
                .AsNoTracking();

            // extract ActivedConstellationNum from AvatarWithConstellationNum
            Reducer<AvatarWithConstellationNum, int, ConcurrentBag<int>> groupReducer = new((input, result) =>
            {
                result
                    .GetOrAdd(input.AvatarId, (_) => new ConcurrentBag<int>())
                    .Add(input.ActivedNum);
            });

            // [角色id,未相加的命座数]
            groupReducer.Reduce(avatars);

            ConcurrentBag<AvatarConstellationNum> calculationResult = new();

            Parallel.ForEach(groupReducer.ReduceResult, avatarIdAllConstellationCount =>
            {
                int currentAvatarCount = 0;

                Reducer<int, int, int> reducer = new((constellation, result) =>
                {
                    result.AddOrUpdate(constellation, 1, (_, previousValue) => Interlocked.Increment(ref previousValue));
                    Interlocked.Increment(ref currentAvatarCount);
                });

                // [角色id,计数后的命座数]
                reducer.Reduce(avatarIdAllConstellationCount.Value);

                IEnumerable<Rate<int>> rate = reducer.ReduceResult
                    .Select(idCount => new Rate<int>(idCount.Key, (double)idCount.Value / currentAvatarCount));

                calculationResult.Add(new()
                {
                    Avatar = avatarIdAllConstellationCount.Key,
                    Rate = rate,
                    HoldingRate = (double)avatarIdAllConstellationCount.Value.Count / totalPlayerCount,
                });
            });

            await statisticsProvider.SaveStatistics<ActivedConstellationNumCalculator>(calculationResult);
        }
    }

    internal record AvatarWithConstellationNum(int AvatarId, int ActivedNum);
}
