using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Services.StatisticCalculation;
using System.Reflection;

namespace Snap.Genshin.Website.Configurations
{
    public class GenshinStatisticsServiceConfiguration
    {
        public GenshinStatisticsServiceConfiguration()
        {
            CalculatorTypes = new();
        }

        public List<Type> CalculatorTypes { get; private set; }

        private readonly Type[] calculatorConstructorFilter = new Type[] { typeof(ApplicationDbContext) };

        public GenshinStatisticsServiceConfiguration AddCalculator<T>() where T : IStatisticCalculator
        {
            return AddCalculator(typeof(T));
        }

        public GenshinStatisticsServiceConfiguration AddCalculator(Type calculatorType)
        {
            if (!calculatorType.IsAssignableTo(typeof(IStatisticCalculator)))
                throw new InvalidCastException($"{nameof(calculatorType)}必须实现{nameof(IStatisticCalculator)}。");

            CalculatorTypes.Add(calculatorType);

            return this;
        }
    }
}
