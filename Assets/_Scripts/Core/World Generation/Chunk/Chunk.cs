namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public struct Chunk
    {
        public ChunkData chunkData;
        public MeshData meshData;

        public Chunk(ChunkData chunkData, MeshData meshData)
        {
            this.chunkData = chunkData;
            this.meshData = meshData;
        }
    }
}