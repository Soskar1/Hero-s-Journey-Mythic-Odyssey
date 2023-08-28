using System;
using System.Collections.Generic;
using HerosJourney.Core.WorldGeneration.Voxels;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class MeshDataBuilder : IDisposable
    {
        private readonly VoxelDataTSStorage _storage;

        private NativeList<JobHandle> _scheduledJobs;
        private Dictionary<int3, MeshData> _generatedMeshData = new Dictionary<int3, MeshData>();
        private List<IDisposable> _toDispose = new List<IDisposable>();

        public MeshDataBuilder(VoxelDataTSStorage storage) 
        {
            _storage = storage;
            _scheduledJobs = new NativeList<JobHandle>(Allocator.Persistent);
        }

        public void Dispose() => _scheduledJobs.Dispose();

        public void ScheduleMeshGenerationJob(WorldData worldData, List<int3> chunkRenderersToCreate)
        {
            _generatedMeshData.Clear();

            foreach (int3 position in chunkRenderersToCreate)
            {
                ChunkData chunkData = worldData.ExistingChunks[position];

                MeshData meshData = new MeshData
                {
                    vertices = new NativeList<int3>(Allocator.TempJob),
                    triangles = new NativeList<int>(Allocator.TempJob),
                };

                TSChunkData TSchunkData = chunkData;
                worldData.ExistingChunks.TryGetValue(chunkData.WorldPosition + new int3(-chunkData.Length, 0, 0), out ChunkData leftChunk);
                TSChunkData TSforwardChunk = worldData.ExistingChunks[position + new int3(0, 0, chunkData.Length)];
                TSChunkData TSrightChunk = worldData.ExistingChunks[position + new int3(chunkData.Length, 0, 0)];
                TSChunkData TSbackChunk = worldData.ExistingChunks[position + new int3(0, 0, -chunkData.Length)];
                TSChunkData TSleftChunk = worldData.ExistingChunks[position + new int3(-chunkData.Length, 0, 0)];

                var neighbourChunks = new MeshGenerationJob.NeighbourChunks
                {
                    forwardChunk = TSforwardChunk,
                    rightChunk = TSrightChunk,
                    leftChunk = TSleftChunk,
                    backChunk = TSbackChunk
                };

                var voxelGeometry = new MeshGenerationJob.VoxelGeometry
                {
                    vertices = VoxelGeometry.vertices,
                    triangles = VoxelGeometry.triangles,
                };

                MeshGenerationJob job = new MeshGenerationJob
                {
                    meshData = meshData,
                    voxelGeometry = voxelGeometry,
                    chunkData = TSchunkData,
                    neighbourChunks = neighbourChunks,
                    storage = _storage.Copy
                };

                JobHandle jobHandle = job.Schedule();
                _generatedMeshData.Add(chunkData.WorldPosition, meshData);
                _scheduledJobs.Add(jobHandle);
                
                _toDispose.Add(TSchunkData);
                _toDispose.Add(TSforwardChunk);
                _toDispose.Add(TSrightChunk);
                _toDispose.Add(TSbackChunk);
                _toDispose.Add(TSleftChunk);
            }
        }

        public Dictionary<int3, MeshData> Complete()
        {
            JobHandle.CompleteAll(_scheduledJobs.AsArray());

            foreach (var data in _toDispose)
                data.Dispose();

            _toDispose.Clear();
            _scheduledJobs.Clear();

            return _generatedMeshData;
        }
    }
}