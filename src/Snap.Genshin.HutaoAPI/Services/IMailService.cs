using Snap.Genshin.Website.Models.Utility;

namespace Snap.Genshin.Website.Services
{
    public interface IMailService
    {
        void SendEmail(IMail mail);
    }
}
