using HerosJourney.Core.WorldGeneration.Chunks;
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

        public static bool IsInBounds(StructureData chunkData, Vector3Int localPosition)
        {
            if (localPosition.x < 0 || localPosition.x >= chunkData.Size.x ||
                localPosition.y < 0 || localPosition.y >= chunkData.Size.y ||
                localPosition.z < 0 || localPosition.z >= chunkData.Size.z)
                return false;

            return true;
        }

        public static Vector3Int WorldToLocalPosition(ChunkData chunk, Vector3Int worldPosition)
        {
            return new Vector3Int
            {
                x = worldPosition.x - chunk.WorldPosition.x,
                y = worldPosition.y - chunk.WorldPosition.y,
                z = worldPosition.z - chunk.WorldPosition.z
            };
        }
    }
}