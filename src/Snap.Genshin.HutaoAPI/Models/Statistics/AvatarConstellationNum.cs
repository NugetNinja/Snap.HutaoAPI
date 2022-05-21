namespace Snap.HutaoAPI.Models.Statistics
{
    public class AvatarConstellationNum
    {
        public int Avatar { get; set; }
        public double HoldingRate { get; set; }
        public IEnumerable<Rate<int>> Rate { get; set; } = null!;
    }
}
