using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public static class ChunkHandler
    {
        public static void SetVoxelAt(ChunkData chunkData, ushort voxelID, int3 localPosition)
        {
            if (IsInBounds(chunkData, localPosition))
            {
                int index = LocalPositionToIndex(chunkData, localPosition);
                chunkData.voxels[index] = voxelID;
            }
        }

        public static bool IsInBounds(ChunkData chunkData, int3 localPosition)
        {
            if (localPosition.x < 0 || localPosition.x >= chunkData.ChunkLength ||
                localPosition.y < 0 || localPosition.y >= chunkData.ChunkHeight ||
                localPosition.z < 0 || localPosition.z >= chunkData.ChunkLength)
                return false;

            return true;
        }

        public static int3 WorldToLocalPosition(Chunk chunk, int3 worldPosition)
        {
            return new int3
            {
                x = worldPosition.x - chunk.worldPosition.x,
                y = worldPosition.y - chunk.worldPosition.y,
                z = worldPosition.z - chunk.worldPosition.z
            };
        }

        public static int LocalPositionToIndex(ChunkData chunkData, int3 localPosition)
        {
            return localPosition.x + chunkData.ChunkLength * localPosition.y + chunkData.ChunkHeight * localPosition.z;
        }

        public static int LocalPositionToIndex(byte chunkLength, byte chunkHeight, int3 localPosition)
        {
            return localPosition.x + chunkLength * localPosition.y + chunkHeight * localPosition.z;
        }

        public static int3 IndexToLocalPosition(ChunkData chunkData, int index)
        {
            int x = index % chunkData.ChunkLength;
            int y = (index / chunkData.ChunkLength) % chunkData.ChunkLength;
            int z = index / (chunkData.ChunkLength * chunkData.ChunkHeight);
            return new int3(x, y, z);
        }

        public static int3 IndexToLocalPosition(byte chunkLength, byte chunkHeight, int index)
        {
            int x = index % chunkLength;
            int y = (index / chunkLength) % chunkLength;
            int z = index / (chunkLength * chunkHeight);
            return new int3(x, y, z);
        }
    }
}