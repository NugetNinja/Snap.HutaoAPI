using Snap.Genshin.Website.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.Genshin.Website.Entities
{
    /// <summary>
    /// API调用者
    /// </summary>
    public class User : IUser
    {
        [NotMapped]
        public Guid UniqueUserId => AppId;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public Guid AppId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public IDictionary<string, string> GetUserInfo()
             => new Dictionary<string, string>
                {
                    { "name", Name },
                };
    }
}
