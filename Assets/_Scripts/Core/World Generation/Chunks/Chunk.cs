using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class Chunk
    {
        public ChunkData chunkData;
        public ChunkRenderer chunkRenderer;
        public Vector3Int worldPosition;

        public Chunk(Vector3Int worldPosition)
        {
            this.worldPosition = worldPosition;
            chunkData = null;
            chunkRenderer = null;
        }
    }
}