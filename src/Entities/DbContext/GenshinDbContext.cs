using Microsoft.EntityFrameworkCore;
using Snap.Genshin.Website.Entities.Record;

namespace Snap.Genshin.Website.Entities
{
    public partial class ApplicationDbContext : DbContext
    {
        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<AvatarDetail> AvatarDetails { get; set; } = null!;
        public DbSet<ReliquarySetDetail> ReliquarySetDetails { get; set; } = null!;
        public DbSet<PlayerRecord> PlayerRecords { get; set; } = null!;
        public DbSet<SpiralAbyssBattle> SpiralAbyssBattles { get; set; } = null!;
        public DbSet<SpiralAbyssLevel> SpiralAbyssLevels { get; set; } = null!;
    }
}
