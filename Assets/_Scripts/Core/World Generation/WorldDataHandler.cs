using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public static class WorldDataHandler
    {
        public static Voxel GetVoxelInWorld(WorldData worldData, Vector3Int worldPosition)
        {
            Vector3Int chunkPosition = GetChunkPosition(worldData, worldPosition);
            ChunkData chunk = null;

            if (worldData.chunks.TryGetValue(chunkPosition, out chunk))
                return ChunkDataHandler.GetVoxelAt(chunk, ChunkDataHandler.WorldToLocalPosition(chunk, worldPosition));

            return null;
        }

        public static Vector3Int GetChunkPosition(WorldData worldData, Vector3Int worldPosition)
        {
            return new Vector3Int
            {
                x = Mathf.FloorToInt(worldPosition.x / (float)worldData.chunkLength) * worldData.chunkLength,
                y = Mathf.FloorToInt(worldPosition.y / (float)worldData.chunkHeight) * worldData.chunkHeight,
                z = Mathf.FloorToInt(worldPosition.z / (float)worldData.chunkLength) * worldData.chunkLength
            };
        }
    }
}