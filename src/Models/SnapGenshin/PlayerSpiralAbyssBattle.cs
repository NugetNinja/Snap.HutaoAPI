namespace Snap.Genshin.Website.Models.SnapGenshin
{
    public class PlayerSpiralAbyssBattle
    {
        /// <summary>
        /// 第几次战斗
        /// </summary>
        public int BattleIndex { get; set; }

        /// <summary>
        /// 出场角色Id列表
        /// </summary>
        public List<int> AvatarIds { get; set; } = null!;
    }
}