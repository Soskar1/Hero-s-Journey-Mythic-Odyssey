using HerosJourney.Core.WorldGeneration;
using HerosJourney.Core.WorldGeneration.Biomes;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class WorldInstaller : MonoInstaller
    {
        [SerializeField] private World _world;
        [SerializeField] private ChunkLoading _chunkLoading;
        [SerializeField] private BiomeGenerator _biomeGenerator;

        public override void InstallBindings()
        {
            BindWorldGenerators();
            BindWorld();
            BindChunkLoading();
        }

        private void BindWorldGenerators()
        {
            Container
                .Bind<BiomeGenerator>()
                .FromInstance(_biomeGenerator)
                .AsSingle();

            Container
                .Bind<TerrainGenerator>()
                .AsSingle();
        }

        private void BindWorld()
        {
            Container
                .Bind<World>()
                .FromInstance(_world)
                .AsSingle();
        }

        private void BindChunkLoading()
        {
            Container
                .Bind<ChunkLoading>()
                .FromInstance(_chunkLoading)
                .AsSingle();
        }
    }
}