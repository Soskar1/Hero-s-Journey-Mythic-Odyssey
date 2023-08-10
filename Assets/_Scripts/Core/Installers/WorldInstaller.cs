using HerosJourney.Core.WorldGeneration;
using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Structures;
using HerosJourney.Core.WorldGeneration.Terrain;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class WorldInstaller : MonoInstaller
    {
        [Header("World Generation Settings")]
        [SerializeField] private int _chunkLength;
        [SerializeField] private int _chunkHeight;
        [SerializeField] private int _worldSeed;
        [SerializeField, Range(1, 32)] private int _renderDistance;

        [Header("World Generators")]
        [SerializeField] private TerrainGenerator _terrainGenerator;
        [SerializeField] private StructureGenerator _structureGenerator;

        [Header("World")]
        [SerializeField] private World _world;

        [Header("Chunk Loader")]
        [SerializeField] private ChunkLoader _chunkLoader;

        public override void InstallBindings()
        {
            BindWorldGenerationSettings();
            BindWorldGenerators();
            BindWorld();
            BindChunkLoader();
        }

        private void BindWorldGenerationSettings()
        {
            Container
                .Bind<WorldGenerationSettings>()
                .AsSingle()
                .WithArguments(new WorldData(_chunkLength, _chunkHeight, _worldSeed), _renderDistance);
        }

        private void BindWorldGenerators()
        {
            Container
                .Bind<IGenerator>()
                .To<TerrainGenerator>()
                .FromInstance(_terrainGenerator)
                .AsSingle();

            //Container
            //    .Bind<IGenerator>()
            //    .To<StructureGenerator>()
            //    .FromInstance(_structureGenerator)
            //    .AsSingle();

            Container
                .BindInterfacesAndSelfTo<ChunkGenerator>()
                .AsSingle();
        }

        private void BindWorld()
        {
            Container
                .Bind<World>()
                .FromInstance(_world)
                .AsSingle();
        }

        private void BindChunkLoader()
        {
            Container
                .Bind<ChunkLoader>()
                .FromInstance(_chunkLoader)
                .AsSingle();
        }
    }
}