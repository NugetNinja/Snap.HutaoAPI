using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Models;
using Snap.Genshin.Website.Models.ApiRequest;
using Snap.Genshin.Website.Models.Utility;
using Snap.Genshin.Website.Services;

namespace Snap.Genshin.Website.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(
            ApplicationDbContext dbContext,
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
        /// <returns>Ok-成功 Unauthorized-密码错误 BadRequest-用户不存在</returns>
        [AllowAnonymous]
        [HttpPost("[Action]")]
        public IActionResult Login([FromBody] LoginModel request)
        {
            IQueryable<User> userQuery = dbContext.Users.Where(u => request.AppId == u.AppId);

            // 用户不存在
            if (!userQuery.Any())
            {
                return this.Fail("登陆失败，用户不存在");
            }

            User user = userQuery.First();
            secretManager.Bind(user.UniqueUserId);
            string savedSecret = secretManager.GetSymmetricSecret("app-secret");

            // 密码不存在
            if (string.IsNullOrEmpty(savedSecret))
            {
                logger.LogError("user {uid} exists, but has no secret.", user.UniqueUserId);
                return NotFound();
            }

            string accessToken = tokenFactory.CreateAccessToken(user);

            return secretManager.HashCompare(request.Secret, savedSecret) ?
                this.Success("登录成功", new
                {
                    AccessToken = accessToken,
                }) :
                this.Fail("登陆失败，密码错误");
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <returns>Ok-成功 BadRequest-查看具体消息 Unauthorized-验证码错误</returns>
        [AllowAnonymous]
        [HttpPost("[Action]")]
        public IActionResult Register([FromBody] RegisterModel request)
        {
            IQueryable<User> userQuery = dbContext.Users.Where(u => request.AppName == u.Name);
            string verifyCodeKey = $"_VERIFY_CODE_{request.Signature}_{request.AppName}";

            // 名称已注册
            if (userQuery.Any())
            {
                cache.Remove(verifyCodeKey);
                return BadRequest(new { Message = "名称已注册" });
            }

            // 验证验证码
            bool codeExists = cache.TryGetValue<string>(verifyCodeKey, out string? storedCode);
            if (!codeExists || request.Code != storedCode)
            {
                return Unauthorized(new { Message = "验证码不正确" });
            }

            // 清除验证码缓存
            cache.Remove(verifyCodeKey);

            // 执行注册
            User user = new() { Name = request.AppName };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            // 生成AppSecret
            string secretBase = Guid.NewGuid().ToString();
            string secret = secretManager.HashSecret(secretBase);

            secretManager.Bind(user.AppId);
            secretManager.StorageSecret("app-secret", secretManager.HashSecret(secret));

            return Ok(new { AppId = user.AppId, AppSecret = secret });
        }

        /// <summary>
        /// 发送验证邮件
        /// </summary>
        /// <returns>Ok(Signature)-成功 BadRequest-请求频繁</returns>
        [AllowAnonymous]
        [HttpPost("[Action]")]
        public IActionResult EmailVerify([FromBody] EmailVerifyModel request)
        {
            // 判断是否请求频繁
            string timeoutFlagKey = $"_MAIL_BUSY_{request.AppName}";
            bool isBusy = cache.TryGetValue<int>(timeoutFlagKey, out _);
            if (isBusy)
            {
                return BadRequest(new { Message = "请求过于频繁" });
            }
            // 生成验证码Key
            string guid = Guid.NewGuid().ToString();
            string verifyCodeKey = $"_VERIFY_CODE_{guid}_{request.AppName}";
            // 缓存验证码，10分钟有效
            string code = GenerateVerifyCode();
            cache.Set(verifyCodeKey, code, TimeSpan.FromMinutes(10));
            // 发送邮件
            mailService.SendEmail(new VerifyCodeMail(code, 10));
            // 写入忙碌标识
            cache.Set(timeoutFlagKey, 1, TimeSpan.FromSeconds(60));

            return Ok(new { Signature = guid });
        }

        private static string GenerateVerifyCode()
        {
            int code = Random.Shared.Next(100000, 999999);
            return code.ToString();
        }
    }
}
