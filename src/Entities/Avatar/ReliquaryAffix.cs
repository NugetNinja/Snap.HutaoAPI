using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Genshin.Website.Entities
{
    /// <summary>
    /// 圣遗物套装效果
    /// </summary>
    public class ReliquaryAffix
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InnerId { get; set; }

        [ForeignKey(nameof(ReliquarySetId))]
        public ReliquarySet ReliquarySet { get; set; } = null!;

        public int ReliquarySetId { get; set; }

        public int ActivationNumber { get; set; }

        public string Effect { get; set; } = null!;
    }
}
