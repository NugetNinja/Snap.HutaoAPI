using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Genshin.Website.Entities.Record
{
    /// <summary>
    /// 深渊关卡信息
    /// </summary>
    public class SpiralAbyssLevel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public long InnerId { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        [ForeignKey(nameof(RecordId))]
        public PlayerRecord Record { get; set; } = null!;
        public long RecordId { get; set; }

        /// <summary>
        /// 层数
        /// </summary>
        [Required]
        public int FloorIndex { get; set; }

        /// <summary>
        /// 房间数
        /// </summary>
        [Required]
        public int LevelIndex { get; set; }

        /// <summary>
        /// 星数
        /// </summary>
        [Required]
        public int Star { get; set; }

        /// <summary>
        /// 战斗详情
        /// </summary>
        public IList<SpiralAbyssBattle> Battles { get; set; } = null!;
    }
}
