using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public static class ChunkHandler
    {
        public static void SetVoxel(ChunkData chunkData, VoxelData voxelData, Vector3Int localPosition)
        {
            if (InBounds(chunkData, localPosition))
                chunkData.voxels[localPosition.x, localPosition.y, localPosition.z].data = voxelData;
        }

        public static VoxelData GetVoxelDataFromChunkCoordinates(ChunkData chunkData, Vector3Int localPosition)
        {
            if (InBounds(chunkData, localPosition))
                return chunkData.voxels[localPosition.x, localPosition.y, localPosition.z].data;

            return null;
        }

        public static bool InBounds(ChunkData chunkData, Vector3Int localPosition)
        {
            if (localPosition.x < 0 || localPosition.x > chunkData.ChunkSize ||
                localPosition.y < 0 || localPosition.y > chunkData.ChunkHeight ||
                localPosition.z < 0 || localPosition.z > chunkData.ChunkSize)
                return false;

            return true;
        }
    }
}