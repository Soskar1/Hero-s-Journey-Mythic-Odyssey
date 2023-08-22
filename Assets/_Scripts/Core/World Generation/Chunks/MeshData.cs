using Unity.Collections;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class MeshData
    {
        public NativeList<float3> vertices;
        public NativeList<int> triangles;
        public NativeList<float3> uvs;

        public MeshData()
        {
            vertices = new NativeList<float3>(Allocator.Persistent);
            triangles = new NativeList<int>(Allocator.Persistent);
            uvs = new NativeList<float3>(Allocator.Persistent);
        }

        public void Dispose()
        {
            vertices.Dispose();
            triangles.Dispose();
            uvs.Dispose();
        }
    }
}