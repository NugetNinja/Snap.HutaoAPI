using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Models.Utility;
using Snap.Genshin.Website.Services;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Snap.Genshin.Website.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(ApplicationDbContext dbContext,
                              ISecretManager secretManager,
                              ILogger<AuthController> logger,
                              IMailService mailService,
                              IMemoryCache cache,
                              ITokenFactory tokenFactory)
        {
            this.secretManager = secretManager;
            this.dbContext = dbContext;
            this.logger = logger;
            this.mailService = mailService;
            this.cache = cache;
            this.tokenFactory = tokenFactory;
        }

        private readonly ISecretManager secretManager;
        private readonly ApplicationDbContext dbContext;
        private readonly ILogger logger;
        private readonly IMailService mailService;
        private readonly IMemoryCache cache;
        private readonly ITokenFactory tokenFactory;

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="appid">AppId</param>
        /// <param name="secret">Secret</param>
        /// <returns>Ok-成功 Unauthorized-密码错误 BadRequest-用户不存在</returns>
        [AllowAnonymous]
        [HttpPost("[Action]")]
        public IActionResult Login([FromForm][Required] Guid appid,
                                   [Required][FromForm] string secret)
        {
            IQueryable<User>? userQuery = dbContext.Users.Where(u => appid == u.AppId);

            // 用户不存在
            if (!userQuery.Any())
            {
                return BadRequest();
            }

            User? user = userQuery.First();
            secretManager.Bind(user.UniqueUserId);
            string? savedSecret = secretManager.GetSymmetricSecret("app-secret");

            // 密码不存在
            if (string.IsNullOrEmpty(savedSecret))
            {
                logger.LogError("user {uid} exists, but has no secret.", user.UniqueUserId);
                return NotFound();
            }

            string? accessToken = tokenFactory.CreateAccessToken(user);
            string? refreshToken = tokenFactory.CreateRefreshToken(user);

            return secretManager.HashCompare(secret, savedSecret) ?
                Ok(new
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken
                }) :
                Unauthorized();
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <param name="signature">验证码识别号</param>
        /// <param name="code">验证码</param>
        /// <returns>Ok-成功 BadRequest-查看具体消息 Unauthorized-验证码错误</returns>
        [AllowAnonymous]
        [HttpPost("[Action]")]
        public IActionResult Register([FromForm, Required] string appName,
                                      [FromForm, Required] string signature,
                                      [FromForm, Required, RegularExpression(@"\d{6}")] string code)
        {
            IQueryable<User>? userQuery = dbContext.Users.Where(u => appName == u.Name);
            string? verifyCodeKey = $"_VERIFY_CODE_{signature}_{appName}";

            // 名称已注册
            if (userQuery.Any())
            {
                cache.Remove(verifyCodeKey);
                return BadRequest(new { Message = "名称已注册" });
            }

            // 验证验证码
            bool codeExists = cache.TryGetValue<string>(verifyCodeKey, out string? storedCode);
            if (!codeExists || code != storedCode)
            {
                return Unauthorized(new { Message = "验证码不正确" });
            }

            // 清除验证码缓存
            cache.Remove(verifyCodeKey);

            // 执行注册
            User? user = new User { Name = appName };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            // 生成AppSecret
            string? secretBase = Guid.NewGuid().ToString();
            string? secret = secretManager.HashSecret(secretBase);

            secretManager.Bind(user.AppId);
            secretManager.StorageSecret("app-secret", secretManager.HashSecret(secret));

            return Ok(new { AppId = user.AppId, AppSecret = secret });
        }

        /// <summary>
        /// 发送验证邮件
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <returns>Ok(Signature)-成功 BadRequest-请求频繁</returns>
        [AllowAnonymous]
        [HttpPost("[Action]")]
        public IActionResult EmailVerify([FromForm, EmailAddress, MaxLength(40)] string appName)
        {
            // 判断是否请求频繁
            string? timeoutFlagKey = $"_MAIL_BUSY_{appName}";
            bool isBusy = cache.TryGetValue<int>(timeoutFlagKey, out _);
            if (isBusy)
            {
                return BadRequest(new { Message = "请求过于频繁" });
            }
            // 生成验证码Key
            string? guid = Guid.NewGuid().ToString();
            string? verifyCodeKey = $"_VERIFY_CODE_{guid}_{appName}";
            // 缓存验证码，10分钟有效
            string? code = GenerateVerifyCode();
            cache.Set(verifyCodeKey, code, TimeSpan.FromMinutes(10));
            // 发送邮件
            mailService.SendEmail(new VerifyCodeMail(code, 10));
            // 写入忙碌标识
            cache.Set(timeoutFlagKey, 1, TimeSpan.FromSeconds(60));

            return Ok(new { Signature = guid });
        }

        /// <summary>
        /// 刷新AccessToken及RefreshToken
        /// </summary>
        /// <returns>新的AccessToken</returns>
        [HttpGet("[Action]")]
        [Authorize(Policy = IdentityPolicyNames.RefreshTokenOnly)]
        public IActionResult RefreshToken()
        {
            IEnumerable<Claim>? userIdQuery = User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier);

            User? user = new User { AppId = Guid.Parse(userIdQuery.Single().Value) };
            string? accessToken = tokenFactory.CreateAccessToken(user);

            // 计算RefreshToken剩余有效期
            DateTime expireTime = User.Claims.Where(c => c.Type == JwtRegisteredClaimNames.Exp)
                                         .Select(c => Convert.ToInt32(c.Value).ToDateTime())
                                         .Single();
            int remainMinutes = (int)(expireTime - DateTime.UtcNow).TotalMinutes;

            // RefreshToken即将过期，则同时刷新AccessToken和RefreshToken
            if (remainMinutes < tokenFactory.RefreshTokenExpireBefore)
            {
                return Ok(new
                {
                    AccessToken = accessToken,
                    RefreshToken = tokenFactory.CreateRefreshToken(user)
                });
            }
            // 只刷新AccessToken
            else
            {
                return Ok(new
                {
                    AccessToken = accessToken
                });
            }
        }

        private static string GenerateVerifyCode()
        {
            int code = Random.Shared.Next(100000, 999999);
            return code.ToString();
        }
    }
}
