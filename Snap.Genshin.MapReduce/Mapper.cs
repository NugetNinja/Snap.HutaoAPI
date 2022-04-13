using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snap.Genshin.MapReduce
{
    public class Mapper<InputType, MapResultType> 
        where InputType : new() 
        where MapResultType : new()
    {
        public Mapper(Func<InputType, MapResultType?> mappingFunc)
        {
            inputBag = new ConcurrentBag<MapResultType>();
            MapResult = new BlockingCollection<MapResultType>(inputBag);
            this.mappingFunc = mappingFunc;
        }

        public void Map(IEnumerable<InputType> fullData)
        {
            if (MapResult.IsAddingCompleted)
            {
                inputBag = new ConcurrentBag<MapResultType>();
                MapResult = new BlockingCollection<MapResultType>(inputBag);
            }
            var chunks = fullData.Chunk(ChunkSize);

            Parallel.ForEach(chunks, chunk =>
            {
                foreach (var input in chunk)
                {
                    var mapResult = mappingFunc(input);
                    if (mapResult is not null) MapResult.Add(mapResult);
                }

                MapResult.CompleteAdding();
            });
        }

        public int ChunkSize { get; set; } = 1000;

        private readonly Func<InputType, MapResultType?> mappingFunc;
        private static ConcurrentBag<MapResultType> inputBag = null!;
        public BlockingCollection<MapResultType> MapResult { get; private set; }
    }
}
