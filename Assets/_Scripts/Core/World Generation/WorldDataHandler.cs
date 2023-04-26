using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace HerosJourney.Core.WorldGeneration
{
    public static class WorldDataHandler
    {
        public static List<Vector3Int> GetChunksAroundPoint(WorldData worldData, Vector3Int worldPosition, int distance)
        {
            List<Vector3Int> chunksAroundPoint = new List<Vector3Int>();

            int xStart = worldPosition.x - worldData.chunkLength * distance;
            int xEnd = worldPosition.x + worldData.chunkLength * distance;
            int zStart = worldPosition.z - worldData.chunkLength * distance;
            int zEnd = worldPosition.z + worldData.chunkLength * distance;

            for (int x = xStart; x <= xEnd; x += worldData.chunkLength)
            {
                for (int z = zStart; z <= zEnd; z += worldData.chunkLength)
                {
                    Vector3Int chunkPosition = GetChunkPosition(worldData, new Vector3Int(x, worldPosition.y, z));
                    chunksAroundPoint.Add(chunkPosition);
                }
            }

            return chunksAroundPoint;
        }

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

        public static List<Vector3Int> ExcludeMatchingChunkPositions(WorldData worldData, List<Vector3Int> chunkPositions)
        {
            return worldData.chunks.Keys
                .Where(pos => chunkPositions.Contains(pos) == false)
                .ToList();
        }

        public static List<Vector3Int> SelectPositionsToCreate(WorldData worldData, List<Vector3Int> chunkPositions, Vector3Int worldPosition)
        {
            return chunkPositions
                .Where(pos => worldData.chunks.ContainsKey(pos) == false)
                .OrderBy(pos => Vector3.Distance(worldPosition, pos))
                .ToList();
        }
    }
}