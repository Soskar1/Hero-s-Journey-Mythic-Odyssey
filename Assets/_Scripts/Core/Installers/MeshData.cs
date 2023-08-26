using Unity.Collections;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration
{
    public struct MeshData
    {
        public NativeList<int3> vertices;
        public NativeList<int> triangles;
    }
}