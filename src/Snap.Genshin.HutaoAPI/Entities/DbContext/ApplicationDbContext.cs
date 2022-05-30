// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft.EntityFrameworkCore;
using Snap.HutaoAPI.Entities.Record;
using Snap.HutaoAPI.Entities.User;

namespace Snap.HutaoAPI.Entities;

/// <summary>
/// 数据库上下文
/// </summary>
public class ApplicationDbContext : DbContext
{
    /// <summary>
    /// 构造一个新的数据库上下文
    /// </summary>
    /// <param name="options">选项</param>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    /// <summary>
    /// 统计json
    /// </summary>
    public DbSet<Statistics> Statistics { get; set; } = null!;

    /// <summary>
    /// 玩家信息
    /// </summary>
    public DbSet<Player> Players { get; set; } = null!;

    /// <summary>
    /// 角色信息
    /// </summary>
    public DbSet<DetailedAvatarInfo> AvatarDetails { get; set; } = null!;

    /// <summary>
    /// 圣遗物信息
    /// </summary>
    public DbSet<DetailedReliquarySetInfo> ReliquarySetDetails { get; set; } = null!;

    /// <summary>
    /// 玩家记录
    /// </summary>
    public DbSet<DetailedRecordInfo> PlayerRecords { get; set; } = null!;

    /// <summary>
    /// 深渊对战信息
    /// </summary>
    public DbSet<DetailedBattleInfo> SpiralAbyssBattles { get; set; } = null!;

    /// <summary>
    /// 深渊关卡信息
    /// </summary>
    public DbSet<DetailedLevelInfo> SpiralAbyssLevels { get; set; } = null!;

    /// <summary>
    /// 深渊角色信息
    /// TODO： 优化掉这个表
    /// </summary>
    public DbSet<SpiralAbyssAvatar> SpiralAbyssAvatars { get; set; } = null!;

    /// <summary>
    /// 物品信息
    /// </summary>
    public DbSet<ItemInfo> GenshinItems { get; set; } = null!;

    /// <summary>
    /// 用户
    /// </summary>
    public DbSet<User> Users { get; set; } = null!;

    /// <summary>
    /// 用户Claims
    /// </summary>
    public DbSet<UserClaim> UsersClaims { get; set; } = null!;

    /// <summary>
    /// 用户Secrets
    /// </summary>
    public DbSet<UserSecret> UsersSecrets { get; set; } = null!;

    /// <inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
