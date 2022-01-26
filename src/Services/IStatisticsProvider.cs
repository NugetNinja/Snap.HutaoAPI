using Snap.Genshin.Website.Services.StatisticCalculation;

namespace Snap.Genshin.Website.Services
{
    public interface IStatisticsProvider
    {
        public Task SaveStatistics<TSource>(object dataObject);
        public Task<object?> ReadStatistics<TSource>();
    }
}
