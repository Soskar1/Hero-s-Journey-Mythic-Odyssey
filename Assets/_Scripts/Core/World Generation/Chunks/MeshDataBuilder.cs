using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HerosJourney.Core.WorldGeneration.Voxels;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class MeshDataBuilder : IInitializable, IDisposable
    {
        private readonly WorldData _worldData;
        private readonly VoxelDataThreadSafeStorage _storage;
        private readonly int _tileSize;
        private readonly float _xStep;
        private readonly float _yStep;

        private NativeList<JobHandle> _scheduledJobs;
        private Dictionary<int3, ThreadSafeMeshData> _generatedThreadSafeMeshData = new Dictionary<int3, ThreadSafeMeshData>();
        private List<IDisposable> _toDispose = new List<IDisposable>();

        public MeshDataBuilder(WorldData worldData, VoxelDataThreadSafeStorage storage, Texture2D textureAtlas, int tileSize) 
        {
            _worldData = worldData;
            _storage = storage;
            _tileSize = tileSize;
            _xStep = _tileSize / (float)textureAtlas.width;
            _yStep = _tileSize / (float)textureAtlas.height;
        }

        public void Initialize() => _scheduledJobs = new NativeList<JobHandle>(Allocator.Persistent);
        public void Dispose() => _scheduledJobs.Dispose();

        public Task<Dictionary<int3, MeshData>> Generate(List<int3> chunkRenderersToCreate)
        {   
            return Task.Run(() =>
            {
                Schedule(chunkRenderersToCreate);
                return Complete();
            });
        }

        private void Schedule(List<int3> chunkRenderersToCreate)
        {
            _generatedThreadSafeMeshData.Clear();

            foreach (int3 position in chunkRenderersToCreate)
            {
                ChunkData chunkData = _worldData.ExistingChunks[position];

                ThreadSafeMeshData meshData = new ThreadSafeMeshData
                {
                    vertices = new NativeList<float3>(Allocator.TempJob),
                    triangles = new NativeList<int>(Allocator.TempJob),
                    uvs = new NativeList<float2>(Allocator.TempJob)
                };

                ThreadSafeChunkData ThreadSafechunkData = chunkData;
                ThreadSafeChunkData ThreadSafeforwardChunk = _worldData.ExistingChunks[position + new int3(0, 0, chunkData.Length)];
                ThreadSafeChunkData ThreadSaferightChunk = _worldData.ExistingChunks[position + new int3(chunkData.Length, 0, 0)];
                ThreadSafeChunkData ThreadSafebackChunk = _worldData.ExistingChunks[position + new int3(0, 0, -chunkData.Length)];
                ThreadSafeChunkData ThreadSafeleftChunk = _worldData.ExistingChunks[position + new int3(-chunkData.Length, 0, 0)];

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
                _generatedThreadSafeMeshData.Add(chunkData.WorldPosition, meshData);
                _scheduledJobs.Add(jobHandle);
                
                _toDispose.Add(ThreadSafechunkData);
                _toDispose.Add(ThreadSafeforwardChunk);
                _toDispose.Add(ThreadSaferightChunk);
                _toDispose.Add(ThreadSafebackChunk);
                _toDispose.Add(ThreadSafeleftChunk);
            }
        }

        private Dictionary<int3, MeshData> Complete()
        {
            Dictionary<int3, MeshData> generatedMeshData = new Dictionary<int3, MeshData>();
            JobHandle.CompleteAll(_scheduledJobs.AsArray());

            foreach (var meshData in _generatedThreadSafeMeshData)
            {
                generatedMeshData.Add(meshData.Key, meshData.Value);
                meshData.Value.Dispose();
            }

            foreach (var data in _toDispose)
                data.Dispose();

            _toDispose.Clear();
            _scheduledJobs.Clear();

            return generatedMeshData;
        }
    }
}