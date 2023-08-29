using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Utils;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        [SerializeField] private WorldRenderer _worldRenderer;
        [SerializeField] private int _renderDistance;
        private WorldData _worldData;
        private TerrainGenerator _terrainGenerator;
        private MeshDataBuilder _meshDataBuilder;

        [Inject]
        private void Construct(WorldData worldData, TerrainGenerator terrainGenerator, MeshDataBuilder meshDataBuilder)
        {
            _worldData = worldData;
            _terrainGenerator = terrainGenerator;
            _meshDataBuilder = meshDataBuilder;
        }

        public void GenerateChunks() => GenerateChunks(int3.zero);

        public async void GenerateChunks(int3 worldPosition)
        {
            WorldGenerationData worldGenerationData = await Task.Run(() => GetWorldGenerationData(worldPosition));
            _terrainGenerator.Generate(worldGenerationData.chunkDataToCreate);

            Dictionary<int3, MeshData> generatedMeshData = _meshDataBuilder.Generate(worldGenerationData.chunkRenderersToCreate);
            RenderChunks(generatedMeshData);
        }

        private void RenderChunks(Dictionary<int3, MeshData> generatedMeshData)
        {
            foreach (var meshData in generatedMeshData)
            {
                ChunkRenderer renderer = _worldRenderer.Dequeue();
                renderer.transform.position = meshData.Key.ToVector3();
                renderer.gameObject.SetActive(true);
                renderer.Render(meshData.Value);
            }
        }

        private WorldGenerationData GetWorldGenerationData(int3 worldPosition)
        {
            List<int3> nearestChunkData = WorldDataExtensions.GetChunksAroundPoint(_worldData, worldPosition, _renderDistance + 1);
            List<int3> nearestChunkRenderers = WorldDataExtensions.GetChunksAroundPoint(_worldData, worldPosition, _renderDistance);

            return new WorldGenerationData
            {
                chunkDataToCreate = nearestChunkData,
                chunkRenderersToCreate = nearestChunkRenderers
            };
        }
    }
}