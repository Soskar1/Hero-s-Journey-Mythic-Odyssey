using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public static class ChunkHandler
    {
        public static bool IsInBounds(ChunkData chunkData, Vector3Int localPosition)
        {
            if (localPosition.x < 0 || localPosition.x >= chunkData.ChunkLength ||
                localPosition.y < 0 || localPosition.y >= chunkData.ChunkHeight ||
                localPosition.z < 0 || localPosition.z >= chunkData.ChunkLength)
                return false;

            return true;
        }

        public static Vector3Int WorldToLocalPosition(Chunk chunk, Vector3Int worldPosition)
        {
            return new Vector3Int
            {
                x = worldPosition.x - chunk.worldPosition.x,
                y = worldPosition.y - chunk.worldPosition.y,
                z = worldPosition.z - chunk.worldPosition.z
            };
        }
    }
}