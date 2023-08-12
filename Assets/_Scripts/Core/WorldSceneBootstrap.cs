using HerosJourney.Core.Entities.PlayableCharacters;
using HerosJourney.Core.WorldGeneration;
using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Voxels;
using Zenject;

namespace HerosJourney.Core
{
    public class WorldSceneBootstrap : IInitializable
    {
        private readonly World _world;
        private readonly PlayerSpawner _spawner;
        private readonly ChunkLoader _chunkLoader;
        private readonly VoxelDataStorage _voxelDataStorage;
        private readonly WorldGenerationSettings _worldGenerationSettings;

        public WorldSceneBootstrap(World world, PlayerSpawner spawner, ChunkLoader chunkLoader,
            VoxelDataStorage voxelDataStorage, WorldGenerationSettings worldGenerationSettings)
        {
            _world = world;
            _spawner = spawner;
            _chunkLoader = chunkLoader;
            _voxelDataStorage = voxelDataStorage;
            _worldGenerationSettings = worldGenerationSettings;
        }

        public void Initialize()
        {
            _world.OnNewChunksInitialized += SpawnPlayer;

            ChunkDataHandler.Initialize(_worldGenerationSettings.WorldData);
            MeshDataBuilder.Initialize(_voxelDataStorage);
        }

        private void SpawnPlayer()
        {
            PlayerFacade player = _spawner.SpawnPlayer();
            _chunkLoader.SetPlayer(player.transform);
            _world.OnNewChunksInitialized -= SpawnPlayer;
        }
    }
}