// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Snap.HutaoAPI.Entities;
using Snap.HutaoAPI.Entities.Record;
using Snap.HutaoAPI.Models;
using Snap.HutaoAPI.Models.Identity;
using Snap.HutaoAPI.Models.Uploading;

namespace Snap.HutaoAPI.Controllers;

/// <summary>
/// 提交记录控制器
/// </summary>
[Route("[controller]")]
[ApiController]
public class RecordController : ControllerBase
{
    private readonly ApplicationDbContext dbContext;

    /// <summary>
    /// 构造一个新的提交记录控制器
    /// </summary>
    /// <param name="dbContext">数据库上下文</param>
    public RecordController(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    /// <summary>
    /// 检查用户是否上传了当期记录
    /// </summary>
    /// <param name="uid">用户uid</param>
    /// <returns>结果</returns>
    [HttpGet("[Action]/{uid}")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ApiExplorerSettings(GroupName = "v1")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<UploadResult>))]
    public IActionResult CheckRecord([FromRoute] string? uid)
    {
        if (!int.TryParse(uid, out _) || uid.Length != 9)
        {
            return this.Fail($"{uid}不是合法的uid");
        }

        IQueryable<Player> playerQuery = dbContext.Players
            .Where(player => player.Uid == uid);

        if (!playerQuery.Any())
        {
            return this.Success("数据库中未找到该Uid", new UploadResult(false));
        }

        // assumes only one uid
        Player player = playerQuery.Single();
        IQueryable<DetailedRecordInfo> recordQuery = dbContext.PlayerRecords
            .Where(record => record.PlayerId == player.InnerId);

        return this.Success("查询成功", new UploadResult(recordQuery.Any()));
    }

    /// <summary>
    /// 获取排行信息
    /// </summary>
    /// <param name="uid">uid</param>
    /// <returns>排行</returns>
    [HttpGet("[Action]/{uid}")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ApiExplorerSettings(GroupName = "v1")]
    [ProducesResponseType(200, Type = typeof(ApiResponse<RankResult>))]
    public IActionResult Rank([FromRoute] string? uid)
    {
        if (!int.TryParse(uid, out _) || uid.Length != 9)
        {
            return this.Fail($"{uid}不是合法的uid");
        }

        SimpleRank? damage = GetRank(uid, RankType.Damage);
        SimpleRank? takeDamage = GetRank(uid, RankType.TakeDamage);

        RankResult result = new()
        {
            Damage = damage,
            TakeDamage = takeDamage,
        };

        return this.Success("获取排行数据成功", result);
    }

    private SimpleRank? GetRank(string uid, RankType rankType)
    {
        // 筛选对应的榜单的全部角色伤害
        IEnumerable<DetailedRankInfo> damageRanks = dbContext.Ranks
            .Include(rank => rank.Player)
            .Where(rank => rank.Type == rankType)
            .OrderBy(rank => rank.Value)
            .AsEnumerable();

        // 挑选出uid对应的伤害
        DetailedRankInfo? damageRank = damageRanks.SingleOrDefault(rank => rank.Player.Uid == uid);

        if (damageRank != null)
        {
            IEnumerable<IndexedRankInfo> indexedDamageRanks = damageRanks
                .Where(rank => rank.AvatarId == damageRank.AvatarId)
                .OrderBy(rank => rank.Value)
                .Select((rank, index) => new IndexedRankInfo(index, rank));

            IndexedRankInfo? indexRank = indexedDamageRanks
                .Single(rank => rank.RankInfo.Player.Uid == uid);

            int damageCount = indexedDamageRanks.Count();

            int uidDamageRank = indexRank.Index + 1;
            double damagePercent = (double)uidDamageRank / damageCount;
            return SimpleRank.Create(indexRank.RankInfo, damagePercent);
        }

        return null;
    }

    /// <summary>
    /// 上传记录
    /// </summary>
    /// <param name="record">记录</param>
    /// <returns>结果</returns>
    [HttpPost("Upload")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<IActionResult> UploadRecord([FromBody] RecordInfo record)
    {
        if (!record.Validate())
        {
            return this.Fail($"数据包含无效的内容");
        }

        Player? player = dbContext.Players
            .Where(player => player.Uid == record.Uid)
            .Include(player => player.Avatars)
            .SingleOrDefault();

        player = await SavePlayerAsync(record, player).ConfigureAwait(false);
        await SaveRecordInfoAsync(record, player).ConfigureAwait(false);
        await SaveRankInfoAsync(record, player).ConfigureAwait(false);

        return this.Success($"UID : {record.Uid}的数据上传成功");
    }

    private async Task SaveRankInfoAsync(RecordInfo record, Player player)
    {
        if (record.DamageMost != null && record.TakeDamageMost != null)
        {
            IQueryable<DetailedRankInfo> oldRankInfos = dbContext.Ranks
                .Where(ranks => ranks.PlayerId == player.InnerId);

            dbContext.Ranks.RemoveRange(oldRankInfos);

            List<DetailedRankInfo> newRankInfos = new(2)
            {
                new()
                {
                    PlayerId = player.InnerId,
                    AvatarId = record.DamageMost.AvatarId,
                    Value = record.DamageMost.Value,
                    Type = RankType.Damage,
                },
                new()
                {
                    PlayerId = player.InnerId,
                    AvatarId = record.TakeDamageMost.AvatarId,
                    Value = record.TakeDamageMost.Value,
                    Type = RankType.TakeDamage,
                },
            };

            dbContext.Ranks.AddRange(newRankInfos);

            await dbContext
                .SaveChangesAsync()
                .ConfigureAwait(false);
        }
    }

    private async Task<Player> SavePlayerAsync(RecordInfo record, Player? player)
    {
        if (player is null)
        {
            player = new Player(record.Uid, new());
            dbContext.Players.Add(player);
        }

        player.Avatars = record.PlayerAvatars
            .Select(avatar => avatar.Complexify())
            .ToList();

        await dbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);
        return player;
    }

    private async Task SaveRecordInfoAsync(RecordInfo record, Player player)
    {
        DetailedRecordInfo? oldPlayerRecord = await dbContext.PlayerRecords
            .Where(record => record.PlayerId == player.InnerId)
            .FirstOrDefaultAsync();

        if (oldPlayerRecord is not null)
        {
            // 删除旧记录
            dbContext.PlayerRecords.Remove(oldPlayerRecord);
        }

        // 插入新记录
        dbContext.PlayerRecords.Add(new DetailedRecordInfo()
        {
            PlayerId = player.InnerId,
            SpiralAbyssLevels = record.PlayerSpiralAbyssesLevels
                .Select(level => level.Complexify())
                .ToList(),
        });

        await dbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);
    }

    private class UploadResult
    {
        public UploadResult(bool periodUploaded)
        {
            PeriodUploaded = periodUploaded;
        }

        public bool PeriodUploaded { get; }
    }

    private class IndexedRankInfo
    {
        public IndexedRankInfo(int index, DetailedRankInfo rankInfo)
        {
            Index = index;
            RankInfo = rankInfo;
        }

        public int Index { get; set; }

        public DetailedRankInfo RankInfo { get; set; }
    }

    private class RankResult
    {
        public SimpleRank? Damage { get; set; }

        public SimpleRank? TakeDamage { get; set; }
    }

    private class SimpleRank
    {
        public int AvatarId { get; set; }

        public int Value { get; set; }

        public double Percent { get; set; }

        public static SimpleRank Create(DetailedRankInfo rankInfo, double percent)
        {
            return new()
            {
                AvatarId = rankInfo.AvatarId,
                Value = rankInfo.Value,
                Percent = percent,
            };
        }
    }
}