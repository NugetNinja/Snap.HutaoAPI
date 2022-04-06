using Flurl.Http;
using Snap.Genshin.Website.Models.Utility;

namespace Snap.Genshin.Website.Services
{
    public class TestMailSender : IMailService
    {
        public TestMailSender(ILogger<TestMailSender> logger)
        {
            this.logger = logger;
        }

        private readonly ILogger logger;

        public void SendEmail(IMail mail)
        {
            logger.LogInformation("Mail: {content}", mail.Content);
            try
            {
                "http://1.13.172.42:25560/v1/LuaApiCaller?qq=2955881280&funcname=SendMsgV2"
                    .PostJsonAsync(new
                    {
                        ToUserUid = 501604732,
                        SendToType = 2,
                        SendMsgType = "TextMsg",
                        Content = mail.Content
                    }).Wait();
            }
            catch (Exception ex)
            {
                logger.LogError("邮件发送失败：{msg}", ex.Message);
            }
        }
    }
}
