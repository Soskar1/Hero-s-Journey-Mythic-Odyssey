using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class Chunk
    {
        public ChunkData data;
        public ChunkRenderer renderer;
        public int3 worldPosition;

        public Chunk(WorldData worldData, int3 worldPosition)
        {
            this.worldPosition = worldPosition;
            data = new ChunkData(worldData);
            renderer = null;
        }
    }
}