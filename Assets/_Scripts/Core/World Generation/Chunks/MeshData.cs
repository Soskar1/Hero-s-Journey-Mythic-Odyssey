using System.Collections.Generic;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class MeshData
    {
        public List<float3> Vertices { get; private set; }
        public List<int> Triangles { get; private set; }
        public List<float3> UVs { get; private set; }

        public MeshData()
        {
            Vertices = new List<float3>();
            Triangles = new List<int>();
            UVs = new List<float3>();
        }

        public void AddVertices(float3[] vertices)
        {
            foreach (var vertex in vertices)
                Vertices.Add(vertex);
        }

        public void CreateQuad()
        {
            Triangles.Add(Vertices.Count - 4);
            Triangles.Add(Vertices.Count - 3);
            Triangles.Add(Vertices.Count - 2);

            Triangles.Add(Vertices.Count - 4);
            Triangles.Add(Vertices.Count - 2);
            Triangles.Add(Vertices.Count - 1);
        }

        public void AddUVCoordinates(float2[] uvCoordinates)
        {
            foreach (var uv in uvCoordinates)
                UVs.Add(new float3(uv.x, uv.y, 0));
        }
    }
}