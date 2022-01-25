namespace Snap.Genshin.Website.Models
{
    public interface IUser
    {
        Guid UniqueUserId { get; }
        IDictionary<string, string> GetUserInfo();
    }
}
