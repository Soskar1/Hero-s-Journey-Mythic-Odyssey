using HerosJourney.Core.WorldGeneration.Chunks;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        [SerializeField] private ChunkRenderer _chunk;
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
            List<ChunkData> generatedChunkData = _terrainGenerator.Generate(_worldData, _chunkDataPositionsToCreate);
            
            foreach (var chunkData in generatedChunkData)
                _worldData.ExistingChunks.Add(chunkData.WorldPosition, chunkData);

            _meshDataBuilder.ScheduleMeshGenerationJob(_worldData, _chunkRendererPositionsToCreate);
            Dictionary<int3, MeshData> generatedMeshData = _meshDataBuilder.Complete();

            foreach (var meshData in generatedMeshData)
                RenderChunk(meshData.Key, meshData.Value);
        }

        private void RenderChunk(int3 position, MeshData meshData)
        {
            ChunkRenderer chunkInstance = Instantiate(_chunk, new Vector3(position.x, position.y, position.z), Quaternion.identity);
            chunkInstance.Render(meshData);
        }
    }
}