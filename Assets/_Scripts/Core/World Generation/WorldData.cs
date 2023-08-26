namespace HerosJourney.Core.WorldGeneration
{
    public class WorldData
    {
        private byte _chunkLength;
        private byte _chunkHeight;

        public byte ChunkLength => _chunkLength;
        public byte ChunkHeight => _chunkHeight;

        public WorldData(byte chunkLength, byte chunkHeight)
        {
            _chunkLength = chunkLength;
            _chunkHeight = chunkHeight;
        }
    }
}