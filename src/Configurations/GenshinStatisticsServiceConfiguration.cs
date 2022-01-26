using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Services.StatisticCalculation;
using System.Reflection;

namespace Snap.Genshin.Website.Configurations
{
    public class GenshinStatisticsServiceConfiguration
    {
        public GenshinStatisticsServiceConfiguration()
        {
            this.CalculatorConstructors = new();
        }

        public List<ConstructorInfo> CalculatorConstructors { get; private set; }
        private readonly Type[] calculatorConstructorFilter = new Type[] { typeof(ApplicationDbContext) };

        public GenshinStatisticsServiceConfiguration AddCalculator<T>() where T : IStatisticCalculator => AddCalculator(typeof(T));

        public GenshinStatisticsServiceConfiguration AddCalculator(Type calculatorType)
        {
            if (!calculatorType.IsSubclassOf(typeof(IStatisticCalculator)))
                throw new InvalidCastException($"{nameof(calculatorType)}必须实现{nameof(IStatisticCalculator)}。");

            var constructor = calculatorType.GetConstructor(this.calculatorConstructorFilter);
            if (constructor is null)
                throw new InvalidCastException($"{nameof(calculatorType)}必须存在接受{nameof(ApplicationDbContext)}的构造器。");

            this.CalculatorConstructors.Add(constructor);

            return this;
        }
    }
}
