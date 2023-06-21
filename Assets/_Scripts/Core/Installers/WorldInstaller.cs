using HerosJourney.Core.WorldGeneration;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class WorldInstaller : MonoInstaller
    {
        [SerializeField] private World _world;

        public override void InstallBindings()
        {
            Container
                .Bind<World>()
                .FromInstance(_world)
                .AsSingle();
        }
    }
}