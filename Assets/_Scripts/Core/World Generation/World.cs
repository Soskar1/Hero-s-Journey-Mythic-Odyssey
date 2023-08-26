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

        [Inject]
        private void Construct(WorldData worldData)
        {
            _worldData = worldData;
        }

        private void OnDisable() => VoxelGeometry.Dispose();

        public void GenerateChunks() => GenerateChunks(int3.zero);

        public void GenerateChunks(int3 position)
        {
            ChunkData chunkData = new ChunkData(_worldData);

            for (int x = 0; x < _worldData.ChunkLength; ++x)
            {
                for (int z = 0; z < _worldData.ChunkLength; ++z)
                {
                    var y = Mathf.FloorToInt(Mathf.PerlinNoise((position.x + x) * 0.15f, (position.z + z) * 0.15f) * _worldData.ChunkHeight);

                    for (int i = 0; i < y; ++i)
                        chunkData.Voxels[VoxelExtensions.GetVoxelIndex(new int3(x, i, z))] = VoxelType.Stone;

                    for (int i = y; i < _worldData.ChunkHeight; ++i)
                        chunkData.Voxels[VoxelExtensions.GetVoxelIndex(new int3(x, i, z))] = VoxelType.Air;
                }
            }

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

            TSchunkData.voxels.Dispose();

            _chunk.Render(meshData);
        }
    }
}