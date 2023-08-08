using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class ChunkGenerator : IDisposable
    {
        private readonly List<IGenerator> _generators;
        private CancellationTokenSource _taskTokenSource = new CancellationTokenSource();

        public Action<Chunk> ChunkGenerated;

        public ChunkGenerator(List<IGenerator> generators) => _generators = generators;

        public void Dispose() => _taskTokenSource.Cancel();

        public Task GenerateChunkData(WorldData worldData, List<Vector3Int> chunkDataPositionsToCreate)
        {
            ConcurrentDictionary<Vector3Int, ChunkData> chunkDataDictionary = AllocateMemoryForChunkData(worldData, chunkDataPositionsToCreate);
            
            foreach (var data in chunkDataDictionary)
                worldData.chunkData.Add(data.Key, data.Value);

            return Task.Run(() => 
            {
                foreach (var generator in _generators)
                    foreach (ChunkData chunkData in chunkDataDictionary.Values)
                        ThreadPool.QueueUserWorkItem((state) => generator.Generate(chunkData));
                        //generator.Generate(chunkData);
            });
        }

        private ConcurrentDictionary<Vector3Int, ChunkData> AllocateMemoryForChunkData(WorldData worldData, List<Vector3Int> chunkDataPositionsToCreate)
        {
            ConcurrentDictionary<Vector3Int, ChunkData> chunkDataDictionary = new ConcurrentDictionary<Vector3Int, ChunkData>();

            foreach (Vector3Int position in chunkDataPositionsToCreate)
            {
                ChunkData newChunkData = new ChunkData(worldData, position);
                chunkDataDictionary.TryAdd(position, newChunkData);
            }

            return chunkDataDictionary;
        }

        public Task GenerateMeshData(List<ChunkData> chunkDataToRender)
        {
            return Task.Run(() =>
            {
                foreach (ChunkData chunkData in chunkDataToRender)
                {
                    if (_taskTokenSource.Token.IsCancellationRequested)
                        _taskTokenSource.Token.ThrowIfCancellationRequested();

                    MeshData meshData = MeshDataBuilder.GenerateMeshData(chunkData);
                    ChunkGenerated?.Invoke(new Chunk(chunkData, meshData));
                }
            }, _taskTokenSource.Token);
        }
    }
}