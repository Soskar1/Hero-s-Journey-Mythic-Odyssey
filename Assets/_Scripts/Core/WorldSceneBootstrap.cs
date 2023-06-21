using HerosJourney.Core.Entities.PlayableCharacters;
using HerosJourney.Core.WorldGeneration;
using Zenject;

namespace HerosJourney.Core
{
    public class WorldSceneBootstrap : IInitializable
    {
        private readonly World _world;
        private readonly PlayerSpawner _spawner;

        public WorldSceneBootstrap(World world, PlayerSpawner spawner)
        {
            _world = world;
            _spawner = spawner;
        }

        public void Initialize()
        {
            _world.OnNewChunksInitialized += SpawnPlayer;
        }

        private void SpawnPlayer()
        {
            _spawner.SpawnPlayer();

            _world.OnNewChunksInitialized -= SpawnPlayer;
        }
    }
}