using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;

namespace HerosJourney.StructureBuilder
{
    public static class StructureDataHandler
    {
        public static void SetVoxelAt(StructureData structureData, Voxel voxel, Vector3Int localPosition)
        {
            if (IsInBounds(structureData, localPosition))
                structureData.voxels[localPosition.x, localPosition.y, localPosition.z] = voxel;
        }

        public static Voxel GetVoxelAt(StructureData structureData, Vector3Int localPosition)
        {
            if (IsInBounds(structureData, localPosition))
                return structureData.voxels[localPosition.x, localPosition.y, localPosition.z];

            return null;
        }

        public static bool IsInBounds(StructureData structureData, Vector3Int localPosition)
        {
            if (localPosition.x < 0 || localPosition.x >= structureData.Size.x ||
                localPosition.y < 0 || localPosition.y >= structureData.Size.y ||
                localPosition.z < 0 || localPosition.z >= structureData.Size.z)
                return false;

            return true;
        }
    }
}