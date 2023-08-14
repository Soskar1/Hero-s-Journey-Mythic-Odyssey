using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
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
            return Task.Run(() =>
            {
                foreach (var generator in _generators)
                {
                    foreach (Vector3Int pos in chunkDataPositionsToCreate)
                    {
                        ChunkData chunkData;

                        if (!worldData.chunkData.ContainsKey(pos))
                        {
                            chunkData = new ChunkData(worldData, pos);
                            worldData.chunkData.Add(pos, chunkData);
                        }
                        else
                        {
                            chunkData = worldData.chunkData[pos];
                        }

                        generator.Generate(chunkData);
                    }
                }
            });
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