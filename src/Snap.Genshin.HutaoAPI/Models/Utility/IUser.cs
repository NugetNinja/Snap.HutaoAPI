namespace Snap.HutaoAPI.Models
{
    public interface IUser
    {
        Guid UniqueUserId { get; }
        IDictionary<string, string> GetUserInfo();
    }
}
