using System.ComponentModel.DataAnnotations;

namespace Snap.HutaoAPI.Models.SnapGenshin
{
    public class PlayerRecord
    {
        /// <summary>
        /// 玩家Uid
        /// </summary>
        [MaxLength(10)]
        public string Uid { get; set; } = null!;

        /// <summary>
        /// 角色信息
        /// </summary>
        public List<PlayerAvatar> PlayerAvatars { get; set; } = null!;

        /// <summary>
        /// 深渊信息
        /// </summary>
        public List<PlayerSpiralAbyssLevel> PlayerSpiralAbyssesLevels { get; set; } = null!;
    }
}