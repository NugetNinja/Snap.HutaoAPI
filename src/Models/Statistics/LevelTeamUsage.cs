using System.Diagnostics.CodeAnalysis;

namespace Snap.Genshin.Website.Models.Statistics
{
    //record 自动实现相等比较
    public record Team
    {
        public Team() { }
        public Team(string upHalf, string downHalf)
        {
            UpHalf = upHalf;
            DownHalf = downHalf;
        }

        public string UpHalf { get; set; } = null!;
        public string DownHalf { get; set; } = null!;
    }

    public record LevelInfo(int Floor, int Index);

    public record LevelTeamUsage(LevelInfo Level, IEnumerable<Rate<Team>> Teams);
}
