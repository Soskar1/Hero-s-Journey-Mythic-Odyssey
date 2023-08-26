using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration
{
    public class MeshDataBuilder
    {
        public MeshData GenerateMeshData(ChunkData chunkData)
        {
            MeshData meshData = new MeshData
            {
                vertices = new NativeList<int3>(Allocator.TempJob),
                triangles = new NativeList<int>(Allocator.TempJob),
            };

            var TSchunkData = new TSChunkData(chunkData);

            var voxelGeometry = new MeshGenerationJob.VoxelGeometry
            {
                vertices = VoxelGeometry.vertices,
                triangles = VoxelGeometry.triangles,
            };

            MeshGenerationJob job = new MeshGenerationJob
            {
                meshData = meshData,
                voxelGeometry = voxelGeometry,
                chunkData = TSchunkData
            };

            JobHandle jobHandle = job.Schedule();
            jobHandle.Complete();

            TSchunkData.Dispose();

            return meshData;
        }
    }
}