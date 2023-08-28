using System;
using Unity.Collections;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration
{
    public struct MeshData : IDisposable
    {
        public NativeList<int3> vertices;
        public NativeList<int> triangles;

        public void Dispose()
        {
            vertices.Dispose();
            triangles.Dispose();
        }
    }
}