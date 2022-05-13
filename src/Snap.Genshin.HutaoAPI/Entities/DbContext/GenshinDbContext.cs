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
        /// players
        /// </summary>
        public DbSet<Player> Players { get; set; } = null!;

        /// <summary>
        /// avatar details
        /// </summary>
        public DbSet<AvatarDetail> AvatarDetails { get; set; } = null!;

        /// <summary>
        /// Reliquary details
        /// </summary>
        public DbSet<ReliquarySetDetail> ReliquarySetDetails { get; set; } = null!;

        /// <summary>
        /// player records
        /// </summary>
        public DbSet<PlayerRecord> PlayerRecords { get; set; } = null!;

        /// <summary>
        /// 深渊对战
        /// </summary>
        public DbSet<SpiralAbyssBattle> SpiralAbyssBattles { get; set; } = null!;

        /// <summary>
        /// 深渊等级
        /// </summary>
        public DbSet<SpiralAbyssLevel> SpiralAbyssLevels { get; set; } = null!;

        /// <summary>
        /// 深渊头像
        /// </summary>
        public DbSet<SpiralAbyssAvatar> SpiralAbyssAvatars { get; set; } = null!;

        /// <summary>
        /// genshin
        /// </summary>
        public DbSet<ItemInfo> GenshinItems { get; set; } = null!;
    }
}
