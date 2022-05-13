namespace Snap.HutaoAPI.Models.Utility
{
    public class VerifyCodeMail : IMail
    {
        public VerifyCodeMail(string code, int expireTime)
        {
            Code = code;
            ExpireTime = expireTime;
        }

        public string Code { get; init; }
        public int ExpireTime { get; init; }

        public string Content
        {
            get => $@"正在进行验证操作，您的验证码是: {Code}, {ExpireTime}分钟内有效。";
        }

        public string Destination
        {
            get => throw new NotImplementedException();
        }

        public string Sender
        {
            get => throw new NotImplementedException();
        }

        public string Title
        {
            get => "SnapGenshin用户验证";
        }
    }
}
