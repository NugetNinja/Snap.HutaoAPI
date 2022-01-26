namespace Snap.Genshin.Website.Models.Utility
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

        public string Content => $@"正在进行验证操作，您的验证码是: {Code}, {ExpireTime}分钟内有效。";

        public string Destination => throw new NotImplementedException();

        public string Sender => throw new NotImplementedException();

        public string Title => "SnapGenshin用户验证";
    }
}
