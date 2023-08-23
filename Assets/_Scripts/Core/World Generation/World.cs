using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Terrain;
using HerosJourney.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        [SerializeField] private WorldRenderer _worldRenderer;

        private WorldGenerationSettings _settings;
        private TerrainGenerator _terrainGenerator;
        private MeshDataBuilder _meshDataBuilder;

        public WorldData WorldData => _settings.WorldData;

        [Inject]
        private void Construct(WorldGenerationSettings settings, TerrainGenerator terrainGenerator, MeshDataBuilder meshDataBuilder)
        {
            _settings = settings;
            _terrainGenerator = terrainGenerator;
            _meshDataBuilder = meshDataBuilder;
        }

        private void OnDisable() => WorldData.Dispose();

        public async void GenerateWorld() => await GenerateWorld(int3.zero);

        private async Task GenerateWorld(int3 worldPosition)
        {
            WorldGenerationData worldGenerationData = await WorldGenerationDataHandler.GenerateWorldGenerationData(_settings, worldPosition);

            UnloadChunks(worldGenerationData.chunkPositionsToRemove);

            Dictionary<int3, ChunkData> generatedChunkData = _terrainGenerator.Generate(worldGenerationData.chunkPositionsToCreate);

            //TODO: Add Structure Generation

            List<Chunk> chunks = GenerateMeshData(generatedChunkData);

            RenderChunks(chunks);

            worldGenerationData.Dispose();
        }

        private List<Chunk> GenerateMeshData(Dictionary<int3, ChunkData> generatedChunkData)
        {
            List<Chunk> chunks = new List<Chunk>();

            foreach (var chunkData in generatedChunkData)
            {
                MeshData meshData = _meshDataBuilder.GenerateMeshData(chunkData.Value);
                Chunk chunk = new Chunk(chunkData.Value, meshData, chunkData.Key);
                chunks.Add(chunk);
                WorldData.existingChunks.Add(chunk.WorldPosition, chunk);
            }
            
            return chunks;
        }

        private void UnloadChunks(NativeList<int3> chunksToRemove)
        {
            foreach (var position in chunksToRemove)
            {
                Chunk chunk = WorldData.existingChunks[position];
                _worldRenderer.Enqueue(chunk.renderer);
                chunk.Dispose();
                WorldData.existingChunks.Remove(position);
            }
        }

        private void RenderChunks(List<Chunk> generatedChunks)
        {
            foreach (var chunk in generatedChunks)
            {
                ChunkRenderer renderer = _worldRenderer.Dequeue();
                chunk.renderer = renderer;
                chunk.Transform.position = chunk.WorldPosition.ToVector3();
                chunk.Render();
            }
        }
    }
}