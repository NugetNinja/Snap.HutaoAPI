using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snap.Genshin.MapReduce
{
    public class Reducer<InputType, OutputKeyType, OutputValueType> where OutputKeyType : notnull
    {
        public Reducer(Action<InputType, ConcurrentDictionary<OutputKeyType, OutputValueType>> reducerAction)
        {
            this.reducerAction = reducerAction;
            this.ReduceResult = new ConcurrentDictionary<OutputKeyType, OutputValueType>();
        }

        public void Reduce(IEnumerable<InputType> inputData)
        {
            Parallel.ForEach(inputData, (input) =>
            {
                reducerAction(input, this.ReduceResult);
            });
        }

        public ConcurrentDictionary<OutputKeyType, OutputValueType> ReduceResult { get; private set; }

        private readonly Action<InputType, ConcurrentDictionary<OutputKeyType, OutputValueType>> reducerAction;
    }
}
