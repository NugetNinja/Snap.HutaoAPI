using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities.Record
{
    /// <summary>
    /// 深渊对战信息
    /// </summary>
    public class SpiralAbyssBattle
    {
        /// <summary>
        /// 对战id from db
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long InnerId { get; set; }

        /// <summary>
        /// 第几次战斗
        /// </summary>
        [Required]
        public int BattleIndex { get; set; }

        /// <summary>
        /// 上场角色的Id
        /// </summary>
        public IList<SpiralAbyssAvatar> Avatars { get; set; } = null!;

        /// <summary>
        /// 外键
        /// </summary>
        [ForeignKey(nameof(SpiralAbyssLevelId))]
        public SpiralAbyssLevel AbyssLevel { get; set; } = null!;

        /// <summary>
        /// 外键 深渊层数
        /// </summary>
        public long SpiralAbyssLevelId { get; set; }
    }
}
