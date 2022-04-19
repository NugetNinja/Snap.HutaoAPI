using Snap.Genshin.Website.Models;
using System.Security.Claims;

namespace Snap.Genshin.Website.Services
{
    public class FakeTokenFactory : ITokenFactory
    {
        public string CreateAccessToken(IUser user) => "hutao-api-fake-token";

        public string CreateToken(IEnumerable<Claim> claims, IUser user, DateTime expires)
        {
            throw new NotImplementedException();
        }
    }
}
