using System.Collections.Generic;
using HerosJourney.Core.WorldGeneration.Chunks;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration
{
    public class WorldData
    {
        public byte ChunkLength { get; private set; }
        public byte ChunkHeight { get; private set; }
        public Dictionary<int3, ChunkData> ExistingChunks { get; private set; }

        public WorldData(byte chunkLength, byte chunkHeight)
        {
            ChunkLength = chunkLength;
            ChunkHeight = chunkHeight;
            ExistingChunks = new Dictionary<int3, ChunkData>();
        }

        public UnsafeHashMap<int3, TSChunkData> GetTSExistingChunks()
        {
            UnsafeHashMap<int3, TSChunkData> tsExistingChunks = new UnsafeHashMap<int3, TSChunkData>(ExistingChunks.Count, Allocator.TempJob);
            foreach (var chunk in ExistingChunks)
                tsExistingChunks.Add(chunk.Key, chunk.Value);

            return tsExistingChunks;
        }
    }
}