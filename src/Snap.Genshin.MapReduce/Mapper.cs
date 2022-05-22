// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Concurrent;

namespace Snap.Genshin.MapReduce
{
    /// <summary>
    /// 映射器
    /// </summary>
    /// <typeparam name="TInput">输入的类型</typeparam>
    /// <typeparam name="TResult">映射结果的类型</typeparam>
    public class Mapper<TInput, TResult>
        where TInput : new()
        where TResult : new()
    {
        private readonly Func<TInput, TResult?> mappingFunc;
        private ConcurrentBag<TResult> inputBag;

        /// <summary>
        /// 构造一个新的映射器
        /// </summary>
        /// <param name="mappingFunc">映射操作</param>
        public Mapper(Func<TInput, TResult?> mappingFunc)
        {
            inputBag = new ConcurrentBag<TResult>();
            Result = new BlockingCollection<TResult>(inputBag);
            this.mappingFunc = mappingFunc;
        }

        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="fullData">待映射的数据</param>
        public void Map(IEnumerable<TInput> fullData)
        {
            if (Result.IsAddingCompleted)
            {
                inputBag = new ConcurrentBag<TResult>();
                Result = new BlockingCollection<TResult>(inputBag);
            }

            IEnumerable<TInput[]> chunks = fullData.Chunk(ChunkSize);

            Parallel.ForEach(chunks, chunk =>
            {
                foreach (TInput input in chunk)
                {
                    if (mappingFunc(input) is TResult mapResult)
                    {
                        Result.Add(mapResult);
                    }
                }

                Result.CompleteAdding();
            });
        }

        /// <summary>
        /// 块大小
        /// </summary>
        public int ChunkSize { get; set; } = 1000;

        /// <summary>
        /// 映射结果
        /// </summary>
        public BlockingCollection<TResult> Result { get; private set; }
    }
}
