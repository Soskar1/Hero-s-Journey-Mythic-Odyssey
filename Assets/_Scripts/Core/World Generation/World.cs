using HerosJourney.Core.WorldGeneration.Chunks;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        private WorldGenerationSettings _settings;

        public WorldData WorldData => _settings.WorldData;

        [Inject]
        private void Construct(WorldGenerationSettings settings) => _settings = settings;

        public async void GenerateWorld() => await Task.Run(() => GenerateWorld(int3.zero));

        private async void GenerateWorld(int3 worldPosition)
        {
            WorldGenerationData worldGenerationData = await WorldGenerationDataHandler.GenerateWorldGenerationData(_settings, worldPosition);
            
            //TODO: Chunk Unloading
            
            foreach (var position in worldGenerationData.chunkPositionsToCreate)
            {
                Chunk chunk = new Chunk(WorldData, position);
                WorldData.existingChunks.Add(position, chunk);
            }

            //TODO: create ChunkData at each nearest chunk position

            //TODO: multithreaded chunk generation

            //TODO: multithreaded MeshData generation

            //TODO: chunk render

            worldGenerationData.Dispose();
        }    
    }
}