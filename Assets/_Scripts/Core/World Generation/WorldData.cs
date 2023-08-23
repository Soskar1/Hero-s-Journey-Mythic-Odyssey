using System.Collections.Generic;
using HerosJourney.Core.WorldGeneration.Chunks;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration
{
    public class WorldData
    {
        public byte chunkLength;
        public byte chunkHeight;

        public Dictionary<int3, Chunk> existingChunks = new Dictionary<int3, Chunk>();

        public WorldData(byte chunkLength, byte chunkHeight)
        {
            this.chunkLength = chunkLength;
            this.chunkHeight = chunkHeight;
        }

        public void Dispose()
        {
            foreach (var chunk in existingChunks.Values)
                chunk.Dispose();
        }
    }
}