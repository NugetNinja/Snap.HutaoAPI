using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Genshin.Website.Entities
{
    /// <summary>
    /// 圣遗物详情
    /// </summary>
    public class ReliquaryDetail
    {
        public long AvatarDetailId { get; set; }
        public int ReliquaryId { get; set; }

        [ForeignKey(nameof(AvatarDetailId))]
        public AvatarDetail AvatarDetail { get; set; } = null!;

        [ForeignKey(nameof(ReliquaryId))]
        public Reliquary Reliquary { get; set; } = null!;

        /// <summary>
        /// 圣遗物等级
        /// </summary>
        public int Level { get; set; }
    }
}
