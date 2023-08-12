using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public static class ChunkDataHandler
    {
        private static WorldData _worldData;

        public static void Initialize(WorldData worldData) => _worldData = worldData;

        public static void SetVoxelAt(ChunkData chunkData, Voxel voxel, Vector3Int localPosition)
        {
            if (IsInBounds(chunkData, localPosition))
                chunkData.voxels[localPosition.x, localPosition.y, localPosition.z] = voxel;
            else
                WorldDataHandler.SetVoxelInWorld(_worldData, voxel, chunkData.WorldPosition + localPosition);
        }

        public static Voxel GetVoxelAt(ChunkData chunkData, Vector3Int localPosition)
        {
            if (IsInBounds(chunkData, localPosition))
                return chunkData.voxels[localPosition.x, localPosition.y, localPosition.z];

            return WorldDataHandler.GetVoxelInWorld(_worldData, chunkData.WorldPosition + localPosition);
        }

        public static bool IsInBounds(ChunkData chunkData, Vector3Int localPosition)
        {
            if (localPosition.x < 0 || localPosition.x >= chunkData.ChunkLength ||
                localPosition.y < 0 || localPosition.y >= chunkData.ChunkHeight ||
                localPosition.z < 0 || localPosition.z >= chunkData.ChunkLength)
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