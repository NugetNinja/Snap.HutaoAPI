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
    /// 上传记录
    /// </summary>
    /// <param name="record">记录</param>
    /// <returns>结果</returns>
    [HttpPost("Upload")]
    [Authorize(IdentityPolicyNames.CommonUser)]
    [ApiExplorerSettings(GroupName = "v1")]
    public async Task<IActionResult> UploadRecord([FromBody] RecordInfo record)
    {
        // TODO validate record.
        Player? player = dbContext.Players
            .Where(player => player.Uid == record.Uid)
            .Include(player => player.Avatars)
            .SingleOrDefault();

        if (player is null)
        {
            player = new Player(record.Uid, new());
            dbContext.Players.Add(player);
        }
        else
        {
            // clear last time uploaded avatars
            player.Avatars.Clear();
        }

        player.Avatars = record.PlayerAvatars
            .Select(avatar => avatar.Complexify())
            .ToList();

        await dbContext
            .SaveChangesAsync()
            .ConfigureAwait(false);

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

        return this.Success($"UID : {record.Uid}的数据上传成功");
    }

    private class UploadResult
    {
        public UploadResult(bool periodUploaded)
        {
            PeriodUploaded = periodUploaded;
        }

        public bool PeriodUploaded { get; }
    }
}
