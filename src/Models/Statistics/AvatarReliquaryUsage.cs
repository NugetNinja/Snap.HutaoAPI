namespace Snap.Genshin.Website.Models.Statistics
{
    /// <summary>
    /// 圣遗物配置数据
    /// </summary>
    public class AvatarReliquaryUsage
    {
        public int Avatar { get; set; }
        public IEnumerable<Rate<string>> ReliquaryUsage { get; set; } = null!;
    }
}
