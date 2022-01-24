using System.Text.Json.Serialization;

namespace DGP.Genshin.MiHoYoAPI.Record.SpiralAbyss
{
    /// <summary>
    /// 间
    /// </summary>
    public class Level
    {
        [JsonPropertyName("index")] public int Index { get; set; }
        [JsonPropertyName("star")] public int Star { get; set; }
        [JsonPropertyName("max_star")] public int MaxStar { get; set; }
        [JsonPropertyName("battles")] public List<Battle>? Battles { get; set; }
    }
}
