using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public static class ChunkVoxelData
    {
        public static MeshData GetChunkMeshData(ChunkData chunkData)
        {
            MeshData meshData = new MeshData();

            for (int x = 0; x < chunkData.ChunkSize; ++x)
                for (int y = 0; y < chunkData.ChunkHeight; ++y)
                    for (int z = 0; z < chunkData.ChunkSize; ++z)
                        meshData = VoxelFaceGeneration.GenerateVoxel(chunkData, meshData, new Vector3Int(x, y, z), chunkData.voxels[x, y, z].type);

            return meshData;
        }

        public static void SetVoxelAt(ChunkData chunkData, Voxel voxel, Vector3Int localPosition)
        {
            if (IsInBounds(chunkData, localPosition))
                chunkData.voxels[localPosition.x, localPosition.y, localPosition.z] = voxel;
        }

        public static Voxel GetVoxelDataAt(ChunkData chunkData, Vector3Int localPosition)
        {
            if (IsInBounds(chunkData, localPosition))
                return chunkData.voxels[localPosition.x, localPosition.y, localPosition.z];

            return null;
        }

        public static bool IsInBounds(ChunkData chunkData, Vector3Int localPosition)
        {
            if (localPosition.x < 0 || localPosition.x >= chunkData.ChunkSize ||
                localPosition.y < 0 || localPosition.y >= chunkData.ChunkHeight ||
                localPosition.z < 0 || localPosition.z >= chunkData.ChunkSize)
                return false;

            return true;
        }
    }
}