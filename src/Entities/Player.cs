using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Genshin.Website.Entities
{
    /// <summary>
    /// 玩家实体
    /// </summary>
    public class Player
    {
        /// <summary>
        /// 玩家Id (from database)
        /// </summary>
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid InnerId { get; set; }

        /// <summary>
        /// 玩家uid (from Mohoyo)
        /// </summary>
        [Required]
        public string Uid { get; set; } = null!;
    }
}
