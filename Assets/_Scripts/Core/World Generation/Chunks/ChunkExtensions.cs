using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public static class ChunkExtensions
    {
        public static bool IsInBounds(TSChunkData chunkData, int3 localPosition)
        {
            if (localPosition.x < 0 || localPosition.x >= chunkData.Length ||
                localPosition.y < 0 || localPosition.y >= chunkData.Height ||
                localPosition.z < 0 || localPosition.z >= chunkData.Length)
                return false;

            return true;
        }
    }
}