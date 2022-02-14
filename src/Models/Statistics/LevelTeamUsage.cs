using System.Diagnostics.CodeAnalysis;

namespace Snap.Genshin.Website.Models.Statistics
{
    [SuppressMessage("","CA1067")]
    public class Team : IEquatable<Team>
    {
        public Team() { }
        public Team(string upHalf, string downHalf)
        {
            UpHalf = upHalf;
            DownHalf = downHalf;
        }

        public string UpHalf { get; set; } = null!;
        public string DownHalf { get; set; } = null!;

        public bool Equals(Team? other)
        {
            return other is not null && (UpHalf == other.UpHalf && DownHalf == other.DownHalf);
        }
    }

    public record LevelInfo(int Floor, int Index);

    public record LevelTeamUsage(LevelInfo Level, IEnumerable<Rate<Team>> Teams);
}
