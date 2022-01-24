using System.ComponentModel.DataAnnotations;

namespace Snap.Genshin.Website.Entities
{
    /// <summary>
    /// 武器实体
    /// </summary>
    public class Weapon
    {
        /// <summary>
        /// 武器Id (from Mihoyo)
        /// </summary>
        [Required, Key]
        public int Id { get; set; }

        /// <summary>
        /// 武器名字
        /// </summary>
        [Required]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 图标
        /// </summary>
        [Required]
        public string Icon { get; set; } = null!;

        /// <summary>
        /// 武器类型Id
        /// </summary>
        [Required]
        public int Type { get; set; }

        /// <summary>
        /// 武器稀有度
        /// </summary>
        [Required]
        public int Rarity { get; set; }

        /// <summary>
        /// 武器类型
        /// </summary>
        [Required]
        public string TypeName { get; set; } = null!;

        /// <summary>
        /// 武器描述
        /// </summary>
        [Required]
        public string Description { get; set; } = null!;
    }
}
