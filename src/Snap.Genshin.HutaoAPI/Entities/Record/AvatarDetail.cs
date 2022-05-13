using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities.Record
{
    /// <summary>
    /// 角色详细信息实体
    /// </summary>
    public class AvatarDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public long InnerId { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; } = null!;
        public Guid PlayerId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        public int AvatarId { get; set; }

        /// <summary>
        /// 角色等级
        /// </summary>
        [Required]
        public int AvatarLevel { get; set; }

        /// <summary>
        /// 已激活命座数
        /// </summary>
        [Required]
        public int ActivedConstellationNum { get; set; }

        /// <summary>
        /// 角色武器Id
        /// </summary>
        [Required]
        public int WeaponId { get; set; }

        /// <summary>
        /// 武器等级
        /// </summary>
        [Required]
        public int WeaponLevel { get; set; }

        /// <summary>
        /// 武器精炼等级
        /// </summary>
        [Required]
        public int AffixLevel { get; set; }

        /// <summary>
        /// 圣遗物配置信息
        /// </summary>
        public IList<ReliquarySetDetail> ReliquarySets { get; set; } = null!;
    }
}
