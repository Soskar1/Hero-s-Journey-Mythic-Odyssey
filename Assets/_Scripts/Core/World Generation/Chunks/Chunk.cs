using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class Chunk
    {
        public ChunkData chunkData;
        public ChunkRenderer chunkRenderer;
        public int3 worldPosition;

        public Chunk(int3 worldPosition)
        {
            this.worldPosition = worldPosition;
            chunkData = null;
            chunkRenderer = null;
        }
    }
}