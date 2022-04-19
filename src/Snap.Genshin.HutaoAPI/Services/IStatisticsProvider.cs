namespace Snap.Genshin.Website.Services
{
    public interface IStatisticsProvider
    {
        public Task SaveStatistics<TSource>(object dataObject);
        public Task<string?> ReadStatistics<TSource>();
    }
}
