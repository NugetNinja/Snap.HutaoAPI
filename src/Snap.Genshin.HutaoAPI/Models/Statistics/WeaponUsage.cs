namespace Snap.Genshin.Website.Models.Statistics
{
    /// <summary>
    /// 武器使用数据
    /// </summary>
    public class WeaponUsage
    {
        public int Avatar { get; set; }

        public IEnumerable<Rate<int>> Weapons { get; set; } = null!;
    }
}
