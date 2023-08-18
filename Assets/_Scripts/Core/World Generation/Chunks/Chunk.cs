using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class Chunk
    {
        public ChunkData chunkData;
        public ChunkRenderer chunkRenderer;
        public int3 worldPosition;

        public Chunk(WorldData worldData, int3 worldPosition)
        {
            this.worldPosition = worldPosition;
            chunkData = new ChunkData(worldData);
            chunkRenderer = null;
        }
    }
}