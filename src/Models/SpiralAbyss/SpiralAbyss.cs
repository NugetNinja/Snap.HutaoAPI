using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DGP.Genshin.MiHoYoAPI.Record.SpiralAbyss
{
    /// <summary>
    /// 深境螺旋信息
    /// </summary>
    public class SpiralAbyss
    {
        [JsonPropertyName("schedule_id")] public int ScheduleId { get; set; }
        [JsonPropertyName("start_time")] public string? StartTime { get; set; }
        [JsonPropertyName("end_time")] public string? EndTime { get; set; }
        [JsonPropertyName("total_battle_times")] public int TotalBattleTimes { get; set; }
        [JsonPropertyName("total_win_times")] public int TotalWinTimes { get; set; }
        [JsonPropertyName("max_floor")] public string? MaxFloor { get; set; }
        [JsonPropertyName("reveal_rank")] public List<Rank>? RevealRank { get; set; }
        [JsonPropertyName("defeat_rank")] public List<Rank>? DefeatRank { get; set; }
        [JsonPropertyName("damage_rank")] public List<Rank>? DamageRank { get; set; }
        [JsonPropertyName("take_damage_rank")] public List<Rank>? TakeDamageRank { get; set; }
        [JsonPropertyName("normal_skill_rank")] public List<Rank>? NormalSkillRank { get; set; }
        [JsonPropertyName("energy_skill_rank")] public List<Rank>? EnergySkillRank { get; set; }
        [JsonPropertyName("floors")] public List<Floor>? Floors { get; set; }
        [JsonPropertyName("total_star")] public int TotalStar { get; set; }
        [JsonPropertyName("is_unlock")] public string? IsUnlock { get; set; }
    }
}
