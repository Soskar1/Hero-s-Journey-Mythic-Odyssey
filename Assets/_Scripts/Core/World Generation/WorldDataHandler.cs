using Unity.Mathematics;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public static class WorldDataHandler
    {
        public static int3 GetChunkPosition(byte chunkLength, byte chunkHeight, int3 worldPosition)
        {
            return new int3
            {
                x = Mathf.FloorToInt(worldPosition.x / (float)chunkLength) * chunkLength,
                y = Mathf.FloorToInt(worldPosition.y / (float)chunkHeight) * chunkHeight,
                z = Mathf.FloorToInt(worldPosition.z / (float)chunkLength) * chunkLength
            };
        }

        public static int3 GetChunkPosition(WorldData worldData, int3 worldPosition)
        {
            return GetChunkPosition(worldData.chunkLength, worldData.chunkHeight, worldPosition);
        }
    }
}