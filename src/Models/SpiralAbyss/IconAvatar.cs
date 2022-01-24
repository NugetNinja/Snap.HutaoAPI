using System.Text.Json.Serialization;

namespace DGP.Genshin.MiHoYoAPI.Record.SpiralAbyss
{
    /// <summary>
    /// 仅包含头像的角色信息
    /// </summary>
    public class IconAvatar
    {
        [JsonPropertyName("id")] public int Id { get; set; }
        [JsonPropertyName("icon")] public string? Icon { get; set; }
        [JsonPropertyName("level")] public int Level { get; set; }
        [JsonPropertyName("rarity")] public int Rarity { get; set; }
    }
}
