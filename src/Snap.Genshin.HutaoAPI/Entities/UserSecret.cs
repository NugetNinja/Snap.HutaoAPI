using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities
{
    /// <summary>
    /// 用户Secret
    /// </summary>
    public class UserSecret
    {
        /// <summary>
        /// Id
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 内容
        /// </summary>
        [Required]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// 用户
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; } = null!;

        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }
    }
}
