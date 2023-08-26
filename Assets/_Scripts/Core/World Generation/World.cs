using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        [SerializeField] private ChunkRenderer _chunk;
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
            ChunkData chunkData = new ChunkData(_worldData, position);
            _terrainGenerator.Generate(chunkData);

            MeshData meshData = _meshDataBuilder.GenerateMeshData(chunkData);
            _chunk.Render(meshData);
        }
    }
}