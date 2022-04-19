namespace Snap.Genshin.Website.Models.SnapGenshin
{
    public class PlayerSpiralAbyssLevel
    {
        /// <summary>
        /// 第几层
        /// </summary>
        public int FloorIndex { get; set; }

        /// <summary>
        /// 第几间
        /// </summary>
        public int LevelIndex { get; set; }

        /// <summary>
        /// 星数
        /// </summary>
        public int Star { get; set; }

        /// <summary>
        /// 战斗详情
        /// </summary>
        public List<PlayerSpiralAbyssBattle> Battles { get; set; } = null!;
    }
}