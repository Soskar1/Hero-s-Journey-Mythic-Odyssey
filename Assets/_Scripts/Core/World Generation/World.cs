using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        private WorldGenerationSettings _settings;

        [Inject]
        private void Construct(WorldGenerationSettings settings) => _settings = settings;

        public async void GenerateWorld() => await Task.Run(() => GenerateWorld(Vector3Int.zero));

        private async void GenerateWorld(Vector3Int worldPosition)
        {
            WorldGenerationData worldGenerationData = await WorldGenerationDataHandler.GenerateWorldGenerationData(_settings, worldPosition);

            Debug.Log("worldGenerationData generated");

            //TODO: create ChunkData at each nearest chunk position

            //TODO: multithreaded chunk generation

            //TODO: multithreaded MeshData generation

            //TODO: chunk render

            worldGenerationData.Dispose();
        }    
    }
}