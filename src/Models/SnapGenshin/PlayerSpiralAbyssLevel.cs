namespace Snap.Genshin.Website.Models.SnapGenshin
{
    public class PlayerSpiralAbyssLevel
    {
        public int FloorIndex { get; set; }
        public int LevelIndex { get; set; }
        public int Star { get; set; }
        public List<PlayerSpiralAbyssBattle> Battles { get; set; } = null!;
    }
}