using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities
{
    /// <summary>
    /// 所有物品集合
    /// </summary>
    public class ItemInfo
    {
        /// <summary>
        /// 物品id from db
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InnerId { get; set; }

        /// <summary>
        /// 物品实际id 根据type可以有相同的
        /// </summary>
        [Required]
        public int Id { get; set; }

        /// <summary>
        /// 物品名称
        /// </summary>
        [Required]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 物品url
        /// </summary>
        [Required]
        public string Url { get; set; } = null!;

        /// <summary>
        /// 物品类别
        /// </summary>
        [Required]
        public string Type { get; set; } = null!;
    }
}