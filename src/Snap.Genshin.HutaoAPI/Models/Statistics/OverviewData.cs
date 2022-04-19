namespace Snap.Genshin.Website.Models.Statistics
{
    public class OverviewData
    {
        /// <summary>
        /// 所有用户数量
        /// </summary>
        public int TotalPlayerCount { get; set; }

        /// <summary>
        /// 当期提交深渊数据用户数量
        /// </summary>
        public int CollectedPlayerCount { get; set; }

        /// <summary>
        /// 当期满星用户数量
        /// </summary>
        public int FullStarPlayerCount { get; set; }
    }
}
