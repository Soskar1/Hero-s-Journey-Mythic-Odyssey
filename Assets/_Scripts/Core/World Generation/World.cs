using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Terrain;
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

        public WorldData WorldData => _settings.WorldData;

        [Inject]
        private void Construct(WorldGenerationSettings settings, TerrainGenerator terrainGenerator)
        {
            _settings = settings;
            _terrainGenerator = terrainGenerator;
        }

        public async void GenerateWorld() => await Task.Run(() => GenerateWorld(int3.zero));

        private async void GenerateWorld(int3 worldPosition)
        {
            WorldGenerationData worldGenerationData = await WorldGenerationDataHandler.GenerateWorldGenerationData(_settings, worldPosition);

            UnloadChunks(worldGenerationData.chunkPositionsToRemove);

            Dictionary<int3, ChunkData> generatedChunks = _terrainGenerator.Generate(worldGenerationData.chunkPositionsToCreate);

            //TODO: Add Structure Generation

            //TODO: multithreaded MeshData generation

            //TODO: chunk render

            worldGenerationData.Dispose();
        }

        private void UnloadChunks(NativeList<int3> chunksToRemove)
        {
            foreach (var position in chunksToRemove)
            {
                _worldRenderer.Enqueue(WorldData.existingChunks[position].renderer);
                WorldData.existingChunks.Remove(position);
            }
        }
    }
}