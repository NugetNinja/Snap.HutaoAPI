using Microsoft.EntityFrameworkCore;

namespace Snap.HutaoAPI.Entities
{
    public partial class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<UserClaim> UsersClaims { get; set; } = null!;
        public DbSet<UserSecret> UsersSecrets { get; set; } = null!;
    }
}
