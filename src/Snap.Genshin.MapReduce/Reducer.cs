// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using Microsoft;
using System.Collections.Concurrent;

namespace Snap.Genshin.MapReduce
{
    /// <summary>
    /// 规约器
    /// 本质上是对 <see cref="Parallel.ForEach{TInput}(IEnumerable{TInput}, Action{TInput})"/> 的包装
    /// </summary>
    /// <typeparam name="TInput">输入的类型</typeparam>
    /// <typeparam name="TOutputKey">输出键的类型</typeparam>
    /// <typeparam name="TOutputValue">输出值的类型</typeparam>
    public class Reducer<TInput, TOutputKey, TOutputValue>
        where TOutputKey : notnull
    {
        private readonly Action<TInput, ConcurrentDictionary<TOutputKey, TOutputValue>> reducerAction;
        private ConcurrentDictionary<TOutputKey, TOutputValue> reduceResult = new();
        private bool reduced = false;

        /// <summary>
        /// 构造一个新的规约器
        /// </summary>
        /// <param name="reduceAction">执行的规约操作</param>
        public Reducer(Action<TInput, ConcurrentDictionary<TOutputKey, TOutputValue>> reduceAction)
        {
            this.reducerAction = reduceAction;
        }

        /// <summary>
        /// 获取规约结果
        /// </summary>
        /// <exception cref="InvalidOperationException">Reduce Result can't be retrived before call Reduce method</exception>
        public ConcurrentDictionary<TOutputKey, TOutputValue> Result
        {
            get
            {
                Verify.Operation(reduced, "Reduce Result can't be retrived before call Reduce method");
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
            // prevent multiple reduce
            if (!reduced)
            {
                Parallel.ForEach(inputData, input => reducerAction(input, Result));
                reduced = true;
            }
        }
    }
}
