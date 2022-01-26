using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Genshin.Website.Entities
{
    public class Statistics
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public long InnerId { get; set; }

        [Required]
        public string Source { get; set; } = null!;

        [Required]
        public string Value { get; set; } = null!;

        [Required]
        public int Period { get; set; }
    }
}
