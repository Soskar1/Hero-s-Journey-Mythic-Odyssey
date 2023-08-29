using System;
using System.Collections.Generic;
using HerosJourney.Core.WorldGeneration.Voxels;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class MeshDataBuilder : IDisposable
    {
        private readonly VoxelDataThreadSafeStorage _storage;
        private readonly int _tileSize;
        private readonly float _xStep;
        private readonly float _yStep;

        private NativeList<JobHandle> _scheduledJobs;
        private Dictionary<int3, MeshData> _generatedMeshData = new Dictionary<int3, MeshData>();
        private List<IDisposable> _toDispose = new List<IDisposable>();

        public MeshDataBuilder(VoxelDataThreadSafeStorage storage, Texture2D textureAtlas, int tileSize) 
        {
            _storage = storage;
            _scheduledJobs = new NativeList<JobHandle>(Allocator.Persistent);

            _tileSize = tileSize;
            _xStep = _tileSize / (float)textureAtlas.width;
            _yStep = _tileSize / (float)textureAtlas.height;
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
                    vertices = new NativeList<float3>(Allocator.TempJob),
                    triangles = new NativeList<int>(Allocator.TempJob),
                    uvs = new NativeList<float2>(Allocator.TempJob)
                };

                ThreadSafeChunkData ThreadSafechunkData = chunkData;
                worldData.ExistingChunks.TryGetValue(chunkData.WorldPosition + new int3(-chunkData.Length, 0, 0), out ChunkData leftChunk);
                ThreadSafeChunkData ThreadSafeforwardChunk = worldData.ExistingChunks[position + new int3(0, 0, chunkData.Length)];
                ThreadSafeChunkData ThreadSaferightChunk = worldData.ExistingChunks[position + new int3(chunkData.Length, 0, 0)];
                ThreadSafeChunkData ThreadSafebackChunk = worldData.ExistingChunks[position + new int3(0, 0, -chunkData.Length)];
                ThreadSafeChunkData ThreadSafeleftChunk = worldData.ExistingChunks[position + new int3(-chunkData.Length, 0, 0)];

                var neighbourChunks = new MeshGenerationJob.NeighbourChunks
                {
                    forwardChunk = ThreadSafeforwardChunk,
                    rightChunk = ThreadSaferightChunk,
                    leftChunk = ThreadSafeleftChunk,
                    backChunk = ThreadSafebackChunk
                };

                var textureData = new MeshGenerationJob.TextureData
                {
                    tileSize = _tileSize,
                    xStep = _xStep,
                    yStep = _yStep
                };

                MeshGenerationJob job = new MeshGenerationJob
                {
                    meshData = meshData,
                    textureData = textureData,
                    chunkData = ThreadSafechunkData,
                    neighbourChunks = neighbourChunks,
                    storage = _storage.Copy
                };

                JobHandle jobHandle = job.Schedule();
                _generatedMeshData.Add(chunkData.WorldPosition, meshData);
                _scheduledJobs.Add(jobHandle);
                
                _toDispose.Add(ThreadSafechunkData);
                _toDispose.Add(ThreadSafeforwardChunk);
                _toDispose.Add(ThreadSaferightChunk);
                _toDispose.Add(ThreadSafebackChunk);
                _toDispose.Add(ThreadSafeleftChunk);
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