using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DGP.Genshin.MiHoYoAPI.Record.SpiralAbyss
{
    /// <summary>
    /// 表示一次战斗
    /// </summary>
    public class Battle
    {
        [JsonPropertyName("index")] public int Index { get; set; }
        [JsonPropertyName("timestamp")] public string? Timestamp { get; set; }
        [JsonPropertyName("avatars")] public List<IconAvatar>? Avatars { get; set; }
        public DateTime? Time
        {
            get
            {
                if (Timestamp is not null)
                {
                    DateTimeOffset dto = DateTimeOffset.FromUnixTimeSeconds(int.Parse(Timestamp));
                    return dto.ToLocalTime().DateTime;
                }
                return null;
            }
        }
    }
}
