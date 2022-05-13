using Snap.HutaoAPI.Models.Utility;

namespace Snap.HutaoAPI.Services
{
    public interface IMailService
    {
        void SendEmail(IMail mail);
    }
}
