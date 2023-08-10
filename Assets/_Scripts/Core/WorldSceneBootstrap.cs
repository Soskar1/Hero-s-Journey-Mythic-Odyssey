using HerosJourney.Core.Entities.PlayableCharacters;
using HerosJourney.Core.WorldGeneration;
using Zenject;

namespace HerosJourney.Core
{
    public class WorldSceneBootstrap : IInitializable
    {
        private readonly World _world;
        private readonly PlayerSpawner _spawner;
        private readonly ChunkLoader _chunkLoader;

        public WorldSceneBootstrap(World world, PlayerSpawner spawner, ChunkLoader chunkLoader)
        {
            _world = world;
            _spawner = spawner;
            _chunkLoader = chunkLoader;
        }

        public void Initialize()
        {
            _world.OnNewChunksInitialized += SpawnPlayer;
        }

        private void SpawnPlayer()
        {
            PlayerFacade player = _spawner.SpawnPlayer();
            _chunkLoader.SetPlayer(player.transform);
            _world.OnNewChunksInitialized -= SpawnPlayer;
        }
    }
}