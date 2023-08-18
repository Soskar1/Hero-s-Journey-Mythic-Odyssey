using System.Linq;
using System.Threading.Tasks;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration
{
    public struct WorldGenerationData
    {
        public NativeList<int3> chunkPositionsToCreate;
        public NativeList<int3> chunkPositionsToRemove;

        public void Dispose()
        {
            chunkPositionsToCreate.Dispose();
            chunkPositionsToRemove.Dispose();
        }
    }

    public static class WorldGenerationDataHandler
    { 
        public static Task<WorldGenerationData> GenerateWorldGenerationData(WorldGenerationSettings settings, int3 worldPosition)
        {
            WorldData worldData = settings.WorldData;

            WorldGenerationData worldGenerationData = new WorldGenerationData
            {
                chunkPositionsToCreate = new NativeList<int3>(Allocator.Persistent),
                chunkPositionsToRemove = new NativeList<int3>(Allocator.Persistent)
            };

            NativeList<int3> nearestChunkPositions = new NativeList<int3>(Allocator.TempJob);
            NativeList<int3> existingChunks = new NativeList<int3>(Allocator.TempJob);

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
        public NativeList<int3> existingChunkPositions;

        public NativeList<int3> nearestChunkPositions;
        public NativeList<int3> chunkPositionsToCreate;
        public NativeList<int3> chunkPositionsToRemove;

        public int3 worldPosition;
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
                    int3 chunkPosition = WorldDataHandler.GetChunkPosition(chunkLength, chunkHeight, new int3(x, worldPosition.y, z));
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