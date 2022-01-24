using System.Text.Json.Serialization;

namespace DGP.Genshin.MiHoYoAPI.Record.SpiralAbyss
{
    /// <summary>
    /// 层
    /// </summary>
    public class Floor
    {
        [JsonPropertyName("index")] public int Index { get; set; }
        [JsonPropertyName("icon")] public string? Icon { get; set; }
        [JsonPropertyName("is_unlock")] public string? IsUnlock { get; set; }
        [JsonPropertyName("settle_time")] public string? SettleTime { get; set; }
        [JsonPropertyName("star")] public int Star { get; set; }
        [JsonPropertyName("max_star")] public int MaxStar { get; set; }
        [JsonPropertyName("levels")] public List<Level>? Levels { get; set; }
    }
}
