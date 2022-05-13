using Snap.HutaoAPI.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Snap.HutaoAPI.Entities
{
    /// <summary>
    /// API调用者
    /// </summary>
    public class User : IUser
    {
        [NotMapped]
        public Guid UniqueUserId
        {
            get => AppId;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
        public Guid AppId { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        public IDictionary<string, string> GetUserInfo()
        {
            return new Dictionary<string, string>
                {
                    { "name", Name },
                };
        }
    }
}
