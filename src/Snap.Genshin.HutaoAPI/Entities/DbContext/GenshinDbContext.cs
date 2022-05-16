using Microsoft.EntityFrameworkCore;
using Snap.HutaoAPI.Entities.Record;

namespace Snap.HutaoAPI.Entities
{
    /// <summary>
    /// db context
    /// </summary>
    public partial class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// 玩家信息
        /// </summary>
        public DbSet<Player> Players { get; set; } = null!;

        /// <summary>
        /// 角色信息
        /// </summary>
        public DbSet<AvatarDetail> AvatarDetails { get; set; } = null!;

        /// <summary>
        /// 圣遗物信息
        /// </summary>
        public DbSet<ReliquarySetDetail> ReliquarySetDetails { get; set; } = null!;

        /// <summary>
        /// 角色上传
        /// </summary>
        public DbSet<PlayerRecord> PlayerRecords { get; set; } = null!;

        /// <summary>
        /// 深渊对战信息
        /// </summary>
        public DbSet<SpiralAbyssBattle> SpiralAbyssBattles { get; set; } = null!;

        /// <summary>
        /// 深渊关卡信息
        /// </summary>
        public DbSet<SpiralAbyssLevel> SpiralAbyssLevels { get; set; } = null!;

        /// <summary>
        /// 深渊角色信息
        /// TODO： 优化掉这个表
        /// </summary>
        public DbSet<SpiralAbyssAvatar> SpiralAbyssAvatars { get; set; } = null!;

        /// <summary>
        /// 物品信息
        /// </summary>
        public DbSet<ItemInfo> GenshinItems { get; set; } = null!;
    }
}
