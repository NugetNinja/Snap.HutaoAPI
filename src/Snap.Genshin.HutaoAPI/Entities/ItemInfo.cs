using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Genshin.Website.Entities
{
    public class ItemInfo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InnerId { get; set; }

        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Url { get; set; } = null!;

        [Required]
        public string Type { get; set; } = null!;
    }
}
