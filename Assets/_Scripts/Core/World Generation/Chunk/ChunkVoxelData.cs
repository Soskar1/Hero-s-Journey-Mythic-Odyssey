using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public static class ChunkVoxelData
    {
        public static void SetVoxelAt(ChunkData chunkData, VoxelData voxelData, Vector3Int localPosition)
        {
            if (IsInBounds(chunkData, localPosition))
                chunkData.voxels[localPosition.x, localPosition.y, localPosition.z].data = voxelData;
        }

        public static VoxelData GetVoxelDataAt(ChunkData chunkData, Vector3Int localPosition)
        {
            if (IsInBounds(chunkData, localPosition))
                return chunkData.voxels[localPosition.x, localPosition.y, localPosition.z].data;

            return null;
        }

        public static bool IsInBounds(ChunkData chunkData, Vector3Int localPosition)
        {
            if (localPosition.x < 0 || localPosition.x > chunkData.ChunkSize ||
                localPosition.y < 0 || localPosition.y > chunkData.ChunkHeight ||
                localPosition.z < 0 || localPosition.z > chunkData.ChunkSize)
                return false;

            return true;
        }
    }
}