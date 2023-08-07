using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class ChunkGenerator
    {
        private List<IGenerator> _generators;

        public ChunkGenerator(List<IGenerator> generators) => _generators = generators;

        public Task GenerateChunkData(WorldData worldData, List<Vector3Int> chunkDataPositionsToCreate)
        {
            ConcurrentDictionary<Vector3Int, ChunkData> chunkDataDictionary = AllocateMemoryForChunkData(worldData, chunkDataPositionsToCreate);
            
            foreach (var data in chunkDataDictionary)
                worldData.chunkData.Add(data.Key, data.Value);

            return Task.Run(() => 
            {
                foreach (var generator in _generators)
                    foreach (ChunkData chunkData in chunkDataDictionary.Values)
                        generator.Generate(chunkData);
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

        public Task<ConcurrentDictionary<Vector3Int, MeshData>> GenerateMeshData(List<ChunkData> chunkDataToRender, CancellationTokenSource taskTokenSource)
        {
            ConcurrentDictionary<Vector3Int, MeshData> dictionary = new ConcurrentDictionary<Vector3Int, MeshData>();

            return Task.Run(() =>
            {
                foreach (ChunkData chunkData in chunkDataToRender)
                {
                    if (taskTokenSource.Token.IsCancellationRequested)
                        taskTokenSource.Token.ThrowIfCancellationRequested();

                    MeshData meshData = MeshDataBuilder.GenerateMeshData(chunkData);
                    dictionary.TryAdd(chunkData.WorldPosition, meshData);
                }

                return dictionary;
            }, taskTokenSource.Token
            );
        }
    }
}