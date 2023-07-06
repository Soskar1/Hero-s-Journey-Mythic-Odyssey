using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace HerosJourney.Core.WorldGeneration
{
    public static class WorldDataHandler
    {
        public static List<Vector3Int> GetChunksAroundPoint(WorldData worldData, Vector3Int worldPosition, int renderDistance)
        {
            List<Vector3Int> chunksAroundPoint = new List<Vector3Int>();

            int xStart = worldPosition.x - worldData.chunkLength * renderDistance;
            int xEnd = worldPosition.x + worldData.chunkLength * renderDistance;
            int zStart = worldPosition.z - worldData.chunkLength * renderDistance;
            int zEnd = worldPosition.z + worldData.chunkLength * renderDistance;

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

        public static void SetVoxelInWorld(WorldData worldData, Voxel voxel, Vector3Int worldPosition)
        {
            Vector3Int chunkPosition = GetChunkPosition(worldData, worldPosition);

            if (worldData.chunkData.TryGetValue(chunkPosition, out ChunkData chunk))
                ChunkDataHandler.SetVoxelAt(chunk, voxel, ChunkDataHandler.WorldToLocalPosition(chunk, worldPosition));
        }

        public static Voxel GetVoxelInWorld(WorldData worldData, Vector3Int worldPosition)
        {
            Vector3Int chunkPosition = GetChunkPosition(worldData, worldPosition);

            if (worldData.chunkData.TryGetValue(chunkPosition, out ChunkData chunk))
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
                .Where(pos => chunkPositions.Contains(pos) == false && worldData.chunkData.ContainsKey(pos))
                .ToList();
        }

        public static List<Vector3Int> SelectChunkRendererPositionsToCreate(WorldData worldData, List<Vector3Int> chunkPositions, Vector3Int worldPosition)
        {
            return chunkPositions
                .Where(pos => worldData.chunkRenderers.ContainsKey(pos) == false)
                .OrderBy(pos => Vector3.Distance(worldPosition, pos))
                .ToList();
        }

        public static List<ChunkData> SelectChunksToRender(WorldData worldData, List<Vector3Int> chunkRendererPositions)
        {
            return worldData.chunkData
                .Where(keyValue => chunkRendererPositions.Contains(keyValue.Key))
                .Select(keyValue => keyValue.Value)
                .ToList();
        }
    }
}