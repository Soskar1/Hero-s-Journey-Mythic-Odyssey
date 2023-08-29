using System.Collections.Generic;
using HerosJourney.Core.WorldGeneration.Chunks;
using Unity.Mathematics;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class WorldData
    {
        public byte ChunkLength { get; private set; }
        public byte ChunkHeight { get; private set; }
        public Dictionary<int3, ChunkData> ExistingChunks { get; private set; }

        public WorldData(byte chunkLength, byte chunkHeight)
        {
            ChunkLength = chunkLength;
            ChunkHeight = chunkHeight;
            ExistingChunks = new Dictionary<int3, ChunkData>();
        }
    }

    public static class WorldDataExtensions
    {
        public static List<int3> GetChunksAroundPoint(WorldData worldData, int3 worldPosition, int renderDistance)
        {
            List<int3> chunksAroundPoint = new List<int3>();

            int xStart = worldPosition.x - worldData.ChunkLength * renderDistance;
            int xEnd = worldPosition.x + worldData.ChunkLength * renderDistance;
            int zStart = worldPosition.z - worldData.ChunkLength * renderDistance;
            int zEnd = worldPosition.z + worldData.ChunkLength * renderDistance;

            for (int x = xStart; x <= xEnd; x += worldData.ChunkLength)
            {
                for (int z = zStart; z <= zEnd; z += worldData.ChunkLength)
                {
                    int3 chunkPosition = GetChunkPosition(worldData, new int3(x, worldPosition.y, z));
                    chunksAroundPoint.Add(chunkPosition);
                }
            }

            return chunksAroundPoint;
        }

        public static int3 GetChunkPosition(WorldData worldData, int3 worldPosition)
        {
            return new int3
            {
                x = Mathf.FloorToInt(worldPosition.x / (float)worldData.ChunkLength) * worldData.ChunkLength,
                y = Mathf.FloorToInt(worldPosition.y / (float)worldData.ChunkHeight) * worldData.ChunkHeight,
                z = Mathf.FloorToInt(worldPosition.z / (float)worldData.ChunkLength) * worldData.ChunkLength
            };
        }
    }
}