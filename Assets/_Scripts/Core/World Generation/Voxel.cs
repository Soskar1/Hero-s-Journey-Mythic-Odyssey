using Unity.Collections;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration
{
    public enum VoxelType : ushort
    {
        Null = 0x0000,
        Air = 0x0001,
        Stone = 0x0002
    }

    public struct VoxelGeometry
    {
        [ReadOnly]
        public static readonly NativeArray<int3> vertices = new NativeArray<int3>(8, Allocator.Persistent)
        {
            [0] = new int3(1, 1, 1),
            [1] = new int3(0, 1, 1),
            [2] = new int3(0, 0, 1),
            [3] = new int3(1, 0, 1),
            [4] = new int3(0, 1, 0),
            [5] = new int3(1, 1, 0),
            [6] = new int3(1, 0, 0),
            [7] = new int3(0, 0, 0)
        };

        [ReadOnly]
        public static readonly NativeArray<int> triangles = new NativeArray<int>(24, Allocator.Persistent)
        {
            [0] = 0, [1] = 1, [2] = 2, [3] = 3,
            [4] = 5, [5] = 0, [6] = 3, [7] = 6,
            [8] = 4, [9] = 5, [10] = 6, [11] = 7,
            [12] = 1, [13] = 4, [14] = 7, [15] = 2,
            [16] = 5, [17] = 4, [18] = 1, [19] = 0,
            [20] = 3, [21] = 2, [22] = 7, [23] = 6
        };

        public static void Dispose()
        {
            vertices.Dispose();
            triangles.Dispose();
        }
    }

    public static class VoxelExtensions
    {
        public static int GetVoxelIndex(int3 position) => position.x + position.y * 16 + position.z * 16 * 128;

        public static bool IsEmpty(this VoxelType voxelType) => voxelType == VoxelType.Air;
    }
}