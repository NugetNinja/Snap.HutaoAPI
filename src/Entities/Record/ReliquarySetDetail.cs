using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Genshin.Website.Entities.Record
{
    /// <summary>
    /// 圣遗物套装信息
    /// </summary>
    public class ReliquarySetDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public long InnerId { get; set; }

        /// <summary>
        /// 套装Id (from Mihoyo)
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 装备该套装的数量
        /// </summary>
        [Required]
        public int Count { get; set; }

        /// <summary>
        /// 格式：{Id}-{Count}
        /// </summary>
        [Required]
        public string UnionId { get; set; } = null!;

        /// <summary>
        /// 外键
        /// </summary>
        [ForeignKey(nameof(AvatarDetailId))]
        public AvatarDetail AvatarInfo { get; set; } = null!;

        public long AvatarDetailId { get; set; }
    }
}
