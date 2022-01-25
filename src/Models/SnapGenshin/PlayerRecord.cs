namespace Snap.Genshin.Website.Models.SnapGenshin
{
    public class PlayerRecord
    {
        public string Uid { get; set; } = null!;
        public List<PlayerAvatar> PlayerAvatars { get; set; } = null!;
        public List<PlayerSpiralAbyssLevel> PlayerSpiralAbyssesLevels { get; set; } = null!;
    }
}