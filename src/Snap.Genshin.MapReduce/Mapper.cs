// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using System.Collections.Concurrent;

namespace Snap.Genshin.MapReduce
{
    /// <summary>
    /// 映射器
    /// </summary>
    /// <typeparam name="TInput">输入的类型</typeparam>
    /// <typeparam name="TMapResult">映射结果的类型</typeparam>
    public class Mapper<TInput, TMapResult>
        where TInput : new()
        where TMapResult : new()
    {
        private readonly Func<TInput, TMapResult?> mappingFunc;
        private /*static?*/ ConcurrentBag<TMapResult> inputBag;

        /// <summary>
        /// 构造一个新的映射器
        /// </summary>
        /// <param name="mappingFunc">映射操作</param>
        public Mapper(Func<TInput, TMapResult?> mappingFunc)
        {
            inputBag = new ConcurrentBag<TMapResult>();
            MapResult = new BlockingCollection<TMapResult>(inputBag);
            this.mappingFunc = mappingFunc;
        }

        /// <summary>
        /// 映射
        /// </summary>
        /// <param name="fullData">待映射的数据</param>
        public void Map(IEnumerable<TInput> fullData)
        {
            if (MapResult.IsAddingCompleted)
            {
                inputBag = new ConcurrentBag<TMapResult>();
                MapResult = new BlockingCollection<TMapResult>(inputBag);
            }

            var chunks = fullData.Chunk(ChunkSize);

            Parallel.ForEach(chunks, chunk =>
            {
                foreach (var input in chunk)
                {
                    var mapResult = mappingFunc(input);
                    if (mapResult is not null)
                    {
                        MapResult.Add(mapResult);
                    }
                }

                MapResult.CompleteAdding();
            });
        }

        /// <summary>
        /// 块大小
        /// </summary>
        public int ChunkSize { get; set; } = 1000;

        /// <summary>
        /// 映射结果
        /// </summary>
        public BlockingCollection<TMapResult> MapResult { get; private set; }
    }
}
