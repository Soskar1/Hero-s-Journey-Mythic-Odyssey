using System.Threading.Tasks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        [SerializeField] private byte _chunkLength;
        [SerializeField] private byte _chunkHeight;
        [SerializeField] [Range(4, 32)] private byte _renderDistance;

        private WorldData _worldData;

        private void Awake() => _worldData = new WorldData(_chunkLength, _chunkHeight);

        public async void GenerateWorld() => await Task.Run(() => GenerateWorld(Vector3Int.zero));

        private async void GenerateWorld(Vector3Int worldPosition)
        {
            WorldGenerationData worldGenerationData = await WorldGenerationDataHandler.GenerateWorldGenerationData(_worldData, worldPosition, _renderDistance);

            foreach (var pos in worldGenerationData.chunkPositionsToCreate)
                Debug.Log(pos);

            //TODO: create ChunkData at each nearest chunk position

            //TODO: multithreaded chunk generation

            //TODO: multithreaded MeshData generation

            //TODO: chunk render

            worldGenerationData.Dispose();
        }    
    }
}