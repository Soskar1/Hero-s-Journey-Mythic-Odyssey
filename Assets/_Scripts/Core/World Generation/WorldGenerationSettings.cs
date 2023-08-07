namespace HerosJourney.Core.WorldGeneration
{
    public class WorldGenerationSettings
    {
        private WorldData _worldData;        
        public WorldData WorldData => _worldData;

        private int _renderDistance;
        public int RenderDistance => _renderDistance;

        public WorldGenerationSettings(WorldData worldData, int renderDistance)
        {
            _worldData = worldData;
            _renderDistance = renderDistance;
        }
    }
}