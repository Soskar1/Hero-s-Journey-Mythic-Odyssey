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
        private MeshDataBuilder _meshDataBuilder;

        public WorldData WorldData => _settings.WorldData;

        [Inject]
        private void Construct(WorldGenerationSettings settings, TerrainGenerator terrainGenerator, MeshDataBuilder meshDataBuilder)
        {
            _settings = settings;
            _terrainGenerator = terrainGenerator;
            _meshDataBuilder = meshDataBuilder;
        }

        public async void GenerateWorld() => await Task.Run(() => GenerateWorld(int3.zero));

        private async void GenerateWorld(int3 worldPosition)
        {
            Debug.Log("start");
            WorldGenerationData worldGenerationData = await WorldGenerationDataHandler.GenerateWorldGenerationData(_settings, worldPosition);

            UnloadChunks(worldGenerationData.chunkPositionsToRemove);

            Dictionary<int3, ChunkData> generatedChunks = _terrainGenerator.Generate(worldGenerationData.chunkPositionsToCreate);

            //TODO: Add Structure Generation

            
            List<MeshData> generatedMeshData = new List<MeshData>();
            foreach (var chunk in generatedChunks.Values)
                generatedMeshData.Add(_meshDataBuilder.GenerateMeshData(chunk));
            Debug.Log("end");

            //TODO: chunk render
            worldGenerationData.Dispose();

            foreach (var meshData in generatedMeshData)
                meshData.Dispose();
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