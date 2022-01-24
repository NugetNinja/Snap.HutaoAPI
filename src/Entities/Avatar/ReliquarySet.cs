using System.ComponentModel.DataAnnotations;

namespace Snap.Genshin.Website.Entities
{
    /// <summary>
    /// 圣遗物套装信息
    /// </summary>
    public class ReliquarySet
    {
        /// <summary>
        /// 套装Id (from Mihoyo)
        /// </summary>
        [Key, Required]
        public int Id { get; set; }

        /// <summary>
        /// 套装名称
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// 套装效果
        /// </summary>
        public IEnumerable<ReliquaryAffix> ReliquaryAffixes { get; set; } = null!;
    }
}
