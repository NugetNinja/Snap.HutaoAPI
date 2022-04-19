namespace Snap.Genshin.Website.Models
{
    public enum ApiCode
    {
        Success = 0,
        Fail = -1,
        // 数据库异常
        DbException = 101,
        // 服务冲突
        ServiceConcurrent = 102,
    }
}
