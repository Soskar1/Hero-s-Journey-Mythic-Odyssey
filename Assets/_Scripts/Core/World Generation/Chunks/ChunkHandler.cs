using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public static class ChunkHandler
    {
        private static byte _chunkLength;
        private static byte _chunkHeight;

        public static void Initialize(WorldData worldData)
        {
            _chunkHeight = worldData.chunkHeight;
            _chunkLength = worldData.chunkLength;
        }

        public static void SetVoxelAt(ChunkData chunkData, ushort voxelID, int3 localPosition)
        {
            if (IsInBounds(localPosition))
            {
                int index = LocalPositionToIndex(localPosition);
                chunkData.voxels[index] = voxelID;
            }
        }

        public static bool IsInBounds(int3 localPosition)
        {
            if (localPosition.x < 0 || localPosition.x >= _chunkLength ||
                localPosition.y < 0 || localPosition.y >= _chunkHeight ||
                localPosition.z < 0 || localPosition.z >= _chunkLength)
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

        public static int LocalPositionToIndex(int3 localPosition)
        {
            return localPosition.x + _chunkLength * localPosition.y + _chunkHeight * localPosition.z;
        }

        public static int3 IndexToLocalPosition(int index)
        {
            int x = index % _chunkLength;
            int y = (index / _chunkLength) % _chunkLength;
            int z = index / (_chunkLength * _chunkHeight);
            return new int3(x, y, z);
        }
    }
}