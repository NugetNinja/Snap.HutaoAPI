using System.ComponentModel.DataAnnotations;

namespace Snap.Genshin.Website.Entities
{
    /// <summary>
    /// 角色实体
    /// </summary>
    public class Avatar
    {
        /// <summary>
        /// 角色ID (from Mihoyo)
        /// </summary>
        [Key, Required]
        public int Id { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Image { get; set; } = null!;

        /// <summary>
        /// 名字
        /// </summary>
        [Required]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 元素
        /// </summary>
        [Required]
        public string Element { get; set; } = null!;

        /// <summary>
        /// 稀有度
        /// </summary>
        [Required]
        public int Rarity { get; set; }
    }
}
