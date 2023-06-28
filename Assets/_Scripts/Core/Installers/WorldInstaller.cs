using HerosJourney.Core.WorldGeneration;
using HerosJourney.Core.WorldGeneration.Structures;
using HerosJourney.Core.WorldGeneration.Terrain;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class WorldInstaller : MonoInstaller
    {
        [SerializeField] private World _world;
        [SerializeField] private ChunkLoading _chunkLoading;
        [SerializeField] private TerrainGenerator _terrainGenerator;
        [SerializeField] private StructureGenerator _structureGenerator;

        public override void InstallBindings()
        {
            BindWorldGenerators();
            BindWorld();
            BindChunkLoading();
        }

        private void BindWorldGenerators()
        {
            Container
                .Bind<TerrainGenerator>()
                .FromInstance(_terrainGenerator)
                .AsSingle();

            Container
                .Bind<StructureGenerator>()
                .FromInstance(_structureGenerator)
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