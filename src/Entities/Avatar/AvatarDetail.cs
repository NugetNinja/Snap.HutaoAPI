using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Genshin.Website.Entities
{
    /// <summary>
    /// 角色详细信息实体
    /// </summary>
    public class AvatarDetail
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
        /// 好感度
        /// </summary>
        public int Fetter { get; set; }

        /// <summary>
        /// 角色等级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 已激活命座数
        /// </summary>
        public int ActivedConstellationNum { get; set; }
    }
}
