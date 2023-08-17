namespace HerosJourney.Core.WorldGeneration
{
    public class WorldGenerationSettings
    {
        private byte _renderDistance;

        public WorldData WorldData { get; }
        public byte RenderDistance => _renderDistance;

        public WorldGenerationSettings(byte chunkLength, byte chunkHeight, byte renderDistance)
        {
            WorldData = new WorldData(chunkLength, chunkHeight);
            _renderDistance = renderDistance;
        }
    }
}