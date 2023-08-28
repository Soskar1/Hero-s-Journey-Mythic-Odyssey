using System;
using Unity.Collections;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration
{
    public struct MeshData : IDisposable
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