using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Genshin.Website.Entities
{
    public class WeaponDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public long InnerId { get; set; }

        public int AvatarId { get; set; }

        public Guid UserId { get; set; }

        [ForeignKey(nameof(AvatarId))]
        public Avatar Avatar { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public Player User { get; set; } = null!;

        /// <summary>
        /// 武器等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 武器突破等级
        /// </summary>
        public int PromoteLevel { get; set; }

        /// <summary>
        /// 武器精炼等级
        /// </summary>
        public int AffixLevel { get; set; }
    }
}
