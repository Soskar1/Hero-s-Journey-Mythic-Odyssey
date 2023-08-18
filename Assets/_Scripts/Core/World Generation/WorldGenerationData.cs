using System.Linq;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public struct WorldGenerationData
    {
        public NativeList<Vector3Int> chunkPositionsToCreate;
        public NativeList<Vector3Int> chunkPositionsToRemove;

        public void Dispose()
        {
            chunkPositionsToCreate.Dispose();
            chunkPositionsToRemove.Dispose();
        }
    }

    public static class WorldGenerationDataHandler
    { 
        public static Task<WorldGenerationData> GenerateWorldGenerationData(WorldGenerationSettings settings, Vector3Int worldPosition)
        {
            WorldData worldData = settings.WorldData;

            WorldGenerationData worldGenerationData = new WorldGenerationData
            {
                chunkPositionsToCreate = new NativeList<Vector3Int>(Allocator.Persistent),
                chunkPositionsToRemove = new NativeList<Vector3Int>(Allocator.Persistent)
            };

            NativeList<Vector3Int> nearestChunkPositions = new NativeList<Vector3Int>(Allocator.TempJob);
            NativeList<Vector3Int> existingChunks = new NativeList<Vector3Int>(Allocator.TempJob);

            return Task.Run(() =>
            {
                foreach (var chunkPosition in worldData.existingChunks.Keys)
                    existingChunks.Add(chunkPosition);

                GenerateWorldGenerationDataJob job = new GenerateWorldGenerationDataJob
                {
                    existingChunkPositions = existingChunks,
                    nearestChunkPositions = nearestChunkPositions,
                    chunkPositionsToCreate = worldGenerationData.chunkPositionsToCreate,
                    chunkPositionsToRemove = worldGenerationData.chunkPositionsToRemove,
                    worldPosition = worldPosition,
                    chunkLength = worldData.chunkLength,
                    chunkHeight = worldData.chunkHeight,
                    renderDistance = settings.RenderDistance
                };

                JobHandle jobHandle = job.Schedule();
                jobHandle.Complete();

                nearestChunkPositions.Dispose();
                existingChunks.Dispose();

                return worldGenerationData;
            });
        }
    }

    [BurstCompile]
    public struct GenerateWorldGenerationDataJob : IJob
    {
        public NativeList<Vector3Int> existingChunkPositions;

        public NativeList<Vector3Int> nearestChunkPositions;
        public NativeList<Vector3Int> chunkPositionsToCreate;
        public NativeList<Vector3Int> chunkPositionsToRemove;

        public Vector3Int worldPosition;
        public byte chunkLength;
        public byte chunkHeight;
        public byte renderDistance;

        public void Execute()
        {
            GetNearestChunks();
            SelectChunksToCreate();
            SelectChunksToRemove();
        }

        private void GetNearestChunks()
        {
            int xStart = worldPosition.x - chunkLength * renderDistance;
            int xEnd = worldPosition.x + chunkLength * renderDistance;
            int zStart = worldPosition.z - chunkLength * renderDistance;
            int zEnd = worldPosition.z + chunkLength * renderDistance;

            for (int x = xStart; x <= xEnd; x += chunkLength)
            {
                for (int z = zStart; z <= zEnd; z += chunkLength)
                {
                    Vector3Int chunkPosition = WorldDataHandler.GetChunkPosition(chunkLength, chunkHeight, new Vector3Int(x, worldPosition.y, z));
                    nearestChunkPositions.Add(chunkPosition);
                }
            }
        }

        private void SelectChunksToCreate()
        {
            foreach (var chunkPosition in nearestChunkPositions)
                if (!existingChunkPositions.Contains(chunkPosition))
                    chunkPositionsToCreate.Add(chunkPosition);
        }

        private void SelectChunksToRemove()
        {
            foreach (var chunkPosition in existingChunkPositions)
                if (!nearestChunkPositions.Contains(chunkPosition))
                    chunkPositionsToRemove.Add(chunkPosition);
        }
    }
}