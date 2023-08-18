using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public static class WorldDataHandler
    {
        public static Vector3Int GetChunkPosition(byte chunkLength, byte chunkHeight, Vector3Int worldPosition)
        {
            return new Vector3Int
            {
                x = Mathf.FloorToInt(worldPosition.x / (float)chunkLength) * chunkLength,
                y = Mathf.FloorToInt(worldPosition.y / (float)chunkHeight) * chunkHeight,
                z = Mathf.FloorToInt(worldPosition.z / (float)chunkLength) * chunkLength
            };
        }

        public static Vector3Int GetChunkPosition(WorldData worldData, Vector3Int worldPosition)
        {
            return GetChunkPosition(worldData.chunkLength, worldData.chunkHeight, worldPosition);
        }
    }
}