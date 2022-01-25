using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Genshin.Website.Entities.Record
{
    public class PlayerRecord
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public long InnerId { get; set; }

        /// <summary>
        /// 记录上传日期
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 外键
        /// </summary>
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; } = null!;
        public Guid PlayerId { get; set; }

        public IList<SpiralAbyssLevel> SpiralAbyssLevels { get; set; } = null!;
    }
}
