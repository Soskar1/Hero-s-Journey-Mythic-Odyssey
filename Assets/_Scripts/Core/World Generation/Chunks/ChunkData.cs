namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class ChunkData
    {
        public ushort[] voxels;

        public byte ChunkLength { get; private set; }
        public byte ChunkHeight { get; private set; }

        public ChunkData(WorldData worldData)
        {
            ChunkLength = worldData.chunkLength;
            ChunkHeight = worldData.chunkHeight;
            voxels = new ushort[worldData.chunkLength * worldData.chunkHeight * worldData.chunkLength];
        }
    }
}