using Snap.Genshin.Website.Models;
using System.Security.Claims;

namespace Snap.Genshin.Website.Services
{
    public interface ITokenFactory
    {
        string CreateToken(IEnumerable<Claim> claims, IUser user, DateTime expires);
        string CreateAccessToken(IUser user);
    }
}