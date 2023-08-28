using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Voxels;
using HerosJourney.Utils;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        [SerializeField] private WorldRenderer _worldRenderer;
        [SerializeField] private List<int3> _chunkDataPositionsToCreate;
        [SerializeField] private List<int3> _chunkRendererPositionsToCreate;
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

        private void OnDisable() => VoxelGeometry.Dispose();

        public void GenerateChunks() => GenerateChunks(int3.zero);

        public void GenerateChunks(int3 position)
        {
            List<ChunkData> generatedChunkData = _terrainGenerator.Generate(_chunkDataPositionsToCreate);
            
            foreach (var chunkData in generatedChunkData)
                _worldData.ExistingChunks.Add(chunkData.WorldPosition, chunkData);

            _meshDataBuilder.ScheduleMeshGenerationJob(_worldData, _chunkRendererPositionsToCreate);
            Dictionary<int3, MeshData> generatedMeshData = _meshDataBuilder.Complete();
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
    }
}