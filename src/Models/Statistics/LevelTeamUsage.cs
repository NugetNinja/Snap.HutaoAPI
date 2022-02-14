namespace Snap.Genshin.Website.Models.Statistics
{
    public class Team
    {
        public Team() { }
        public Team(string upHalf, string downHalf)
        {
            UpHalf = upHalf;
            DownHalf = downHalf;
        }

        public string UpHalf { get; set; } = null!;
        public string DownHalf { get; set; } = null!;

        public override bool Equals(object? obj)
        {
            if (obj is not Team other)
            {
                return false;
            }

            return UpHalf == other.UpHalf && DownHalf == other.DownHalf;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public record LevelInfo(int Floor, int Index);

    public record LevelTeamUsage(LevelInfo Level, IEnumerable<Rate<Team>> Teams);

}
