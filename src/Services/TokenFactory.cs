using Microsoft.IdentityModel.Tokens;
using Snap.Genshin.Website.Configurations;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Snap.Genshin.Website.Services
{
    public class TokenFactory
    {
        public TokenFactory(ApplicationDbContext dbContext, TokenFactoryConfiguration configuration)
        {
            this.configuration = configuration;
            this.dbContext = dbContext;
        }

        private readonly TokenFactoryConfiguration configuration;
        private readonly ApplicationDbContext dbContext;

        public int RefreshTokenExpireBefore => configuration.RefreshTokenBefore;

        public string CreateToken(IEnumerable<Claim> claims, IUser user, DateTime expires)
        {
            claims = claims.Append(new Claim(ClaimTypes.NameIdentifier, user.UniqueUserId.ToString()));
            var credentials = new SigningCredentials
                (
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.SigningKey)),
                    SecurityAlgorithms.HmacSha256
                );
            var token = new JwtSecurityToken
                (
                    issuer: configuration.Issuer,
                    audience: configuration.Audience,
                    notBefore: DateTime.Now,
                    expires: expires,
                    claims: claims,
                    signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string CreateAccessToken(IUser user)
        {
            var expires = DateTime.Now.AddMinutes(configuration.AccessTokenExpire);
            var userInfoClaims = from info in user.GetUserInfo()
                                 select new Claim(info.Key, info.Value);

            var authorizeClaims = (from claim in dbContext.UsersClaims
                                   where claim.UserId == user.UniqueUserId
                                   select new Claim(claim.ClaimType, claim.ClaimValue))
                                  .ToList()
                                  .Append(new Claim("TokenType", "AccessToken"));
            return CreateToken(Enumerable.Concat(userInfoClaims, authorizeClaims), user, expires);
        }

        public string CreateRefreshToken(IUser user)
        {
            var expires = DateTime.Now.AddMinutes(configuration.RefreshTokenExpire);
            var claims = Enumerable.Empty<Claim>()
                                   .Append(new Claim("TokenType", "RefreshToken"));
            return CreateToken(claims, user, expires);
        }
    }
}
