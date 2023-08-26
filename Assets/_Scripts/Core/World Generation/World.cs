using Unity.Collections;
using Unity.Jobs;
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

        [Inject]
        private void Construct(WorldData worldData, TerrainGenerator terrainGenerator)
        {
            _worldData = worldData;
            _terrainGenerator = terrainGenerator;
        }

        private void OnDisable() => VoxelGeometry.Dispose();

        public void GenerateChunks() => GenerateChunks(int3.zero);

        public void GenerateChunks(int3 position)
        {
            ChunkData chunkData = new ChunkData(_worldData, position);
            _terrainGenerator.Generate(chunkData);

            MeshData meshData = new MeshData
            {
                vertices = new NativeList<int3>(Allocator.TempJob),
                triangles = new NativeList<int>(Allocator.TempJob),
            };

            var TSchunkData = new TSChunkData(chunkData);

            var voxelGeometry = new ChunkJob.VoxelGeometry
            {
                vertices = VoxelGeometry.vertices,
                triangles = VoxelGeometry.triangles,
            };

            ChunkJob job = new ChunkJob
            {
                meshData = meshData,
                voxelGeometry = voxelGeometry,
                chunkData = TSchunkData
            };

            JobHandle jobHandle = job.Schedule();
            jobHandle.Complete();

            TSchunkData.Dispose();

            _chunk.Render(meshData);
        }
    }
}