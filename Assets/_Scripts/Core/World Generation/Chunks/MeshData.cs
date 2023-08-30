using System;
using Unity.Collections;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class MeshData
    {
        public float3[] Vertices { get; private set; }
        public int[] Triangles { get; private set; }
        public float2[] UVs { get; private set; }

        public MeshData(ThreadSafeMeshData meshData)
        {
            Vertices = meshData.vertices.ToArray();
            Triangles = meshData.triangles.ToArray();
            UVs = meshData.uvs.ToArray();
        }

        public static implicit operator MeshData(ThreadSafeMeshData meshData) => new MeshData(meshData);
    }

    public struct ThreadSafeMeshData : IDisposable
    {
        public NativeList<float3> vertices;
        public NativeList<int> triangles;
        public NativeList<float2> uvs;

        public void Dispose()
        {
            vertices.Dispose();
            triangles.Dispose();
            uvs.Dispose();
        }
    }
}