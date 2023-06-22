using HerosJourney.Core.WorldGeneration;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class WorldInstaller : MonoInstaller
    {
        [SerializeField] private World _world;
        [SerializeField] private ChunkLoading _chunkLoading;

        public override void InstallBindings()
        {
            Container
                .Bind<World>()
                .FromInstance(_world)
                .AsSingle();

            Container
                .Bind<ChunkLoading>()
                .FromInstance(_chunkLoading)
                .AsSingle();
        }
    }
}