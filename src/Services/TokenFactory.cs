using Microsoft.IdentityModel.Tokens;
using Snap.Genshin.Website.Configurations;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Snap.Genshin.Website.Services
{
    public class TokenFactory : ITokenFactory
    {
        public TokenFactory(ApplicationDbContext dbContext, TokenFactoryConfiguration configuration)
        {
            this.configuration = configuration;
            this.dbContext = dbContext;
        }

        private readonly TokenFactoryConfiguration configuration;
        private readonly ApplicationDbContext dbContext;

        public string CreateToken(IEnumerable<Claim> claims, IUser user, DateTime expires)
        {
            claims = claims.Append(new Claim(ClaimTypes.NameIdentifier, user.UniqueUserId.ToString()));

            SigningCredentials? credentials = new(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.SigningKey)),
                SecurityAlgorithms.HmacSha256);

            JwtSecurityToken? token = new(
                    issuer: configuration.Issuer,
                    audience: configuration.Audience,
                    notBefore: DateTime.UtcNow,
                    expires: expires,
                    claims: claims,
                    signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string CreateAccessToken(IUser user)
        {
            DateTime expires = DateTime.UtcNow.AddMinutes(configuration.AccessTokenExpire);

            IEnumerable<Claim>? userInfoClaims =
                from info in user.GetUserInfo()
                select new Claim(info.Key, info.Value);

            IEnumerable<Claim>? authorizeClaims =
                (from claim in dbContext.UsersClaims
                 where claim.UserId == user.UniqueUserId
                 select new Claim(claim.ClaimType, claim.ClaimValue))
                .ToList();

            return CreateToken(Enumerable.Concat(userInfoClaims, authorizeClaims), user, expires);
        }
    }
}
