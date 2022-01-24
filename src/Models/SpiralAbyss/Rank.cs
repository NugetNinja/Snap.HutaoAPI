using System.Text.Json.Serialization;

namespace DGP.Genshin.MiHoYoAPI.Record.SpiralAbyss
{
    /// <summary>
    /// 角色数值排行信息
    /// </summary>
    public class Rank
    {
        [JsonPropertyName("avatar_id")] public int AvatarId { get; set; }
        [JsonPropertyName("avatar_icon")] public string? AvatarIcon { get; set; }
        [JsonPropertyName("value")] public int Value { get; set; }
        [JsonPropertyName("rarity")] public int Rarity { get; set; }
    }
}
