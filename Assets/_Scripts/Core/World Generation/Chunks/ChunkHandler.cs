using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public static class ChunkHandler
    {
        public static bool IsInBounds(ChunkData chunkData, int3 localPosition)
        {
            if (localPosition.x < 0 || localPosition.x >= chunkData.ChunkLength ||
                localPosition.y < 0 || localPosition.y >= chunkData.ChunkHeight ||
                localPosition.z < 0 || localPosition.z >= chunkData.ChunkLength)
                return false;

            return true;
        }

        public static int3 WorldToLocalPosition(Chunk chunk, int3 worldPosition)
        {
            return new int3
            {
                x = worldPosition.x - chunk.worldPosition.x,
                y = worldPosition.y - chunk.worldPosition.y,
                z = worldPosition.z - chunk.worldPosition.z
            };
        }
    }
}