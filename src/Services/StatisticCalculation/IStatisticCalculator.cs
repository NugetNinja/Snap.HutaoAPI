namespace Snap.Genshin.Website.Services.StatisticCalculation
{
    public interface IStatisticCalculator
    {
        public Task Calculate();

        /// <summary>
        /// 获取深渊期数
        /// </summary>
        /// <returns></returns>
        public static int GetSpiralPeriodId(DateTime time)
        {
            var periodNum = ((time.Year - 2000) * 12 + time.Month) * 2;
            if (time.Day < 16 || time.Day == 16 && (time - time.Date).TotalMinutes < 240) periodNum--;
            if (time.Day == 1 && (time - time.Date).TotalMinutes < 240) periodNum--;

            return periodNum;
        }
    }
}
