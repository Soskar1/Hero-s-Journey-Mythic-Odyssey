using Unity.Collections;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    public enum VoxelType : ushort
    {
        Null = 0x0000,
        Transparent = 0x0001,
        Solid = 0x0002
    }

    public static class VoxelExtensions
    {
        public static int GetVoxelIndex(int3 position) => position.x + position.y * 16 + position.z * 16 * 128;

        public static bool IsEmpty(this VoxelType voxelType) => voxelType == VoxelType.Transparent;
    }
}