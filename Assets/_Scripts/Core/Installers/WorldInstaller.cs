using HerosJourney.Core.WorldGeneration;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class WorldInstaller : MonoInstaller
    {
        [SerializeField] private byte _chunkLength;
        [SerializeField] private byte _chunkHeight;

        public override void InstallBindings()
        {
            BindWorldData();
            BindGenerators();
        }

        private void BindWorldData()
        {
            Container
                .Bind<WorldData>()
                .AsSingle()
                .WithArguments(_chunkLength, _chunkHeight);
        }

        private void BindGenerators()
        {
            Container
                .Bind<TerrainGenerator>()
                .AsSingle();
        }
    }
}