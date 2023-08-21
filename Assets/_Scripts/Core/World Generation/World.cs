using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Terrain;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
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
            
            //TODO: Chunk Unloading
            
            Dictionary<int3, ChunkData> generatedChunks = _terrainGenerator.Generate(worldGenerationData.chunkPositionsToCreate);

            //TODO: create ChunkData at each nearest chunk position

            //TODO: multithreaded chunk generation

            //TODO: multithreaded MeshData generation

            //TODO: chunk render

            worldGenerationData.Dispose();
        }    
    }
}