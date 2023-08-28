using System.Collections.Generic;
using HerosJourney.Core.WorldGeneration.Chunks;
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
    }
}