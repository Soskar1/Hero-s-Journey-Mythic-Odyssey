using Unity.Collections;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public struct MeshData
    {
        public NativeList<float3> Vertices { get; private set; }
        public NativeList<int> Triangles { get; private set; }
        public NativeList<float3> UVs { get; private set; }

        public void Initialize()
        {
            Vertices = new NativeList<float3>(Allocator.Persistent);
            Triangles = new NativeList<int>(Allocator.Persistent);
            UVs = new NativeList<float3>(Allocator.Persistent);
        }

        public void AddVertices(float3[] vertices)
        {
            foreach (var vertex in vertices)
                Vertices.Add(vertex);
        }

        public void CreateQuad()
        {
            Triangles.Add(Vertices.Length - 4);
            Triangles.Add(Vertices.Length - 3);
            Triangles.Add(Vertices.Length - 2);

            Triangles.Add(Vertices.Length - 4);
            Triangles.Add(Vertices.Length - 2);
            Triangles.Add(Vertices.Length - 1);
        }

        public void AddUVCoordinates(float2[] uvCoordinates)
        {
            foreach (var uv in uvCoordinates)
                UVs.Add(new float3(uv.x, uv.y, 0));
        }
    }
}