using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities.Record
{
    // TODO 这个表真的有必要吗
    public class SpiralAbyssAvatar
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public long InnerId { get; set; }

        [Required]
        public int AvatarId { get; set; }

        [ForeignKey(nameof(SpiralAbyssBattleId))]
        public SpiralAbyssBattle SpiralAbyssBattle { get; set; } = null!;
        public long SpiralAbyssBattleId { get; set; }
    }
}
