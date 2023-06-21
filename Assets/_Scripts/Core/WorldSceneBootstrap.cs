using HerosJourney.Core.Entities.PlayableCharacters;
using HerosJourney.Core.WorldGeneration;
using Zenject;

namespace HerosJourney.Core
{
    public class WorldSceneBootstrap : IInitializable
    {
        private readonly World _world;
        private readonly PlayerSpawner _spawner;
        private readonly ChunkLoading _chunkLoading;

        public WorldSceneBootstrap(World world, PlayerSpawner spawner, ChunkLoading chunkLoading)
        {
            _world = world;
            _spawner = spawner;
            _chunkLoading = chunkLoading;
        }

        public void Initialize()
        {
            _world.OnNewChunksInitialized += SpawnPlayer;
        }

        private void SpawnPlayer()
        {
            PlayerFacade player = _spawner.SpawnPlayer();
            _chunkLoading.SetPlayer(player.transform);
            _world.OnNewChunksInitialized -= SpawnPlayer;
        }
    }
}