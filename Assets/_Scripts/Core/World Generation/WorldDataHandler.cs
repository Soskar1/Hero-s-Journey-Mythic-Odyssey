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

            Vector3Int start = new Vector3Int(worldPosition.x - worldData.chunkLength * distance,
                worldPosition.y - worldData.chunkHeight * distance,
                worldPosition.z - worldData.chunkLength * distance);

            Vector3Int end = new Vector3Int(worldPosition.x + worldData.chunkLength * distance,
                worldPosition.z + worldData.chunkLength * distance,
                worldPosition.y + worldData.chunkHeight * distance);

            for (int x = start.x; x <= end.x; x += worldData.chunkLength)
            {
                for (int z = start.z; z <= end.z; z += worldData.chunkLength)
                {
                    for (int y = start.y; y <= end.y; y += worldData.chunkHeight)
                    {
                        Vector3Int chunkPosition = GetChunkPosition(worldData, new Vector3Int(x, y, z));
                        chunksAroundPoint.Add(chunkPosition);
                    }
                }
            }

            return chunksAroundPoint;
        }

        public static Voxel GetVoxelInWorld(WorldData worldData, Vector3Int worldPosition)
        {
            Vector3Int chunkPosition = GetChunkPosition(worldData, worldPosition);
            ChunkData chunk = null;

            if (worldData.chunkData.TryGetValue(chunkPosition, out chunk))
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

        public static List<Vector3Int> ExcludeMatchingChunkDataPositions(WorldData worldData, List<Vector3Int> chunkPositions)
        {
            return worldData.chunkData.Keys
                .Where(pos => chunkPositions.Contains(pos) == false)
                .ToList();
        }

        public static List<Vector3Int> SelectChunkDataPositionsToCreate(WorldData worldData, List<Vector3Int> chunkPositions, Vector3Int worldPosition)
        {
            return chunkPositions
                .Where(pos => worldData.chunkData.ContainsKey(pos) == false)
                .OrderBy(pos => Vector3.Distance(worldPosition, pos))
                .ToList();
        }

        public static List<Vector3Int> ExcludeMatchingChunkRendererPositions(WorldData worldData, List<Vector3Int> chunkPositions)
        {
            return worldData.chunkRenderers.Keys
                .Where(pos => chunkPositions.Contains(pos) == false)
                .ToList();
        }

        public static List<Vector3Int> SelectChunkRendererPositionsToCreate(WorldData worldData, List<Vector3Int> chunkPositions, Vector3Int worldPosition)
        {
            return chunkPositions
                .Where(pos => worldData.chunkRenderers.ContainsKey(pos) == false)
                .OrderBy(pos => Vector3.Distance(worldPosition, pos))
                .ToList();
        }
    }
}