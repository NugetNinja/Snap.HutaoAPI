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
        }
    }
}
