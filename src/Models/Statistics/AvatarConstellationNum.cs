namespace Snap.Genshin.Website.Models.Statistics
{
    public class AvatarConstellationNum
    {
        public int Avatar { get; set; }

        public IEnumerable<Rate<int>> Rate { get; set; } = null!;
    }
}
