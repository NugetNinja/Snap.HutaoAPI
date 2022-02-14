using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Services.StatisticCalculation;

namespace Snap.Genshin.Website.Configurations
{
    public class GenshinStatisticsServiceConfiguration
    {
        public List<Type> CalculatorTypes { get; } = new();

        private readonly Type[] calculatorConstructorFilter = new Type[] { typeof(ApplicationDbContext) };

        public GenshinStatisticsServiceConfiguration AddCalculator<T>() where T : IStatisticCalculator
        {
            return AddCalculator(typeof(T));
        }

        private GenshinStatisticsServiceConfiguration AddCalculator(Type calculatorType)
        {
            if (!calculatorType.IsAssignableTo(typeof(IStatisticCalculator)))
            {
                throw new InvalidCastException($"{nameof(calculatorType)}必须实现{nameof(IStatisticCalculator)}。");
            }

            CalculatorTypes.Add(calculatorType);

            return this;
        }
    }
}