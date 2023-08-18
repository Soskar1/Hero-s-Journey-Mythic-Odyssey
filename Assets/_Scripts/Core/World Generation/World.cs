using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        private WorldGenerationSettings _settings;

        [Inject]
        private void Construct(WorldGenerationSettings settings) => _settings = settings;

        public async void GenerateWorld() => await Task.Run(() => GenerateWorld(int3.zero));

        private async void GenerateWorld(int3 worldPosition)
        {
            WorldGenerationData worldGenerationData = await WorldGenerationDataHandler.GenerateWorldGenerationData(_settings, worldPosition);
            Debug.Log("WorldGenerationData generated");
            //TODO: create ChunkData at each nearest chunk position

            //TODO: multithreaded chunk generation

            //TODO: multithreaded MeshData generation

            //TODO: chunk render

            worldGenerationData.Dispose();
        }    
    }
}