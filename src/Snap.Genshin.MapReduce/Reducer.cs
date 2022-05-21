// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft;
using System.Collections.Concurrent;

namespace Snap.Genshin.MapReduce
{
    /// <summary>
    /// 规约器
    /// </summary>
    /// <typeparam name="TInput">输入的类型</typeparam>
    /// <typeparam name="TOutputKey">输出键的类型</typeparam>
    /// <typeparam name="TOutputValue">输出值的类型</typeparam>
    public class Reducer<TInput, TOutputKey, TOutputValue>
        where TOutputKey : notnull
    {
        private readonly Action<TInput, ConcurrentDictionary<TOutputKey, TOutputValue>> reducerAction;
        private ConcurrentDictionary<TOutputKey, TOutputValue> reduceResult;
        private bool reduced = false;

        /// <summary>
        /// 构造一个新的规约器
        /// </summary>
        /// <param name="reducerAction">执行的规约操作</param>
        public Reducer(Action<TInput, ConcurrentDictionary<TOutputKey, TOutputValue>> reducerAction)
        {
            this.reducerAction = reducerAction;
            ReduceResult = new ConcurrentDictionary<TOutputKey, TOutputValue>();
        }

        /// <summary>
        /// 获取规约结果
        /// </summary>
        /// <exception cref="InvalidOperationException">Reduce Result can't be retrived before call Reduce method</exception>
        public ConcurrentDictionary<TOutputKey, TOutputValue> ReduceResult
        {
            get
            {
                Verify.Operation(!reduced, "Reduce Result can't be retrived before call Reduce method");
                return reduceResult;
            }

            private set => reduceResult = value;
        }

        /// <summary>
        /// 规约
        /// </summary>
        /// <param name="inputData">输入数据</param>
        public void Reduce(IEnumerable<TInput> inputData)
        {
            Parallel.ForEach(inputData, input => reducerAction(input, ReduceResult));
            reduced = true;
        }
    }
}
