using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Genshin.Website.Entities
{
    /// <summary>
    /// 圣遗物
    /// </summary>
    public class Reliquary
    {
        /// <summary>
        /// 圣遗物Id (from Mihoyo)
        /// </summary>
        [Key, Required]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; } = null!;

        /// <summary>
        /// 位置Id
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// 稀有度
        /// </summary>

        public int Rarity { get; set; }

        /// <summary>
        /// 位置名称
        /// </summary>
        public string PositionName { get; set; } = null!;

        public int ReliquarySetId { get; set; }

        /// <summary>
        /// 套装信息
        /// </summary>
        [ForeignKey(nameof(ReliquarySetId))]
        public ReliquarySet ReliquarySet { get; set; } = null!;
    }
}
