using Flurl.Http;
using Snap.HutaoAPI.Models.Identity;

namespace Snap.HutaoAPI.Services
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
                "http://1.13.172.42:25560/v1/LuaApiCaller?qq=501604732&funcname=SendMsgV2"
                    .PostJsonAsync(new
                    {
                        ToUserUid = 664120433,
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
