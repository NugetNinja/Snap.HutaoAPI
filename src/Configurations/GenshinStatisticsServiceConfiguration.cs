// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Snap.Genshin.Website.Entities;
using Snap.Genshin.Website.Services.StatisticCalculation;

namespace Snap.Genshin.Website.Configurations
{
    public class GenshinStatisticsServiceConfiguration
    {
        public List<Type> CalculatorTypes { get; } = new();

        private readonly Type[] calculatorConstructorFilter = new Type[] { typeof(ApplicationDbContext) };

        public GenshinStatisticsServiceConfiguration AddCalculator<T>()
            where T : IStatisticCalculator
        {
            return AddCalculator(typeof(T));
        }

        private GenshinStatisticsServiceConfiguration AddCalculator(Type calculatorType)
        {
            // basically can't happen, remove this?
            if (!calculatorType.IsAssignableTo(typeof(IStatisticCalculator)))
            {
                throw new InvalidCastException($"{nameof(calculatorType)}必须实现{nameof(IStatisticCalculator)}。");
            }

            CalculatorTypes.Add(calculatorType);

            return this;
        }
    }
}