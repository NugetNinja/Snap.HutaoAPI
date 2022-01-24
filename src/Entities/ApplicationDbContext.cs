using Microsoft.EntityFrameworkCore;

namespace Snap.Genshin.Website.Entities
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Player> Players { get; set; } = null!;
        public DbSet<Avatar> Avatars { get; set; } = null!;
        public DbSet<AvatarDetail> AvatarDetails { get; set; } = null!;
        public DbSet<Reliquary> Reliquarys { get; set; } = null!;
        public DbSet<ReliquaryAffix> ReliquaryAffixes { get; set; } = null!;
        public DbSet<ReliquaryDetail> ReliquaryDetails { get; set; } = null!;
        public DbSet<ReliquarySet> ReliquarySets { get; set; } = null!;
        public DbSet<Weapon> Weapons { get; set; } = null!;
        public DbSet<WeaponDetail> WeaponsDetails { get; set; } = null!;

    }
}
