using HerosJourney.Core.WorldGeneration;
using HerosJourney.Core.WorldGeneration.Noise;
using HerosJourney.Core.WorldGeneration.Terrain;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class WorldInstaller : MonoInstaller
    {
        [SerializeField] private byte _chunkLength;
        [SerializeField] private byte _chunkHeight;
        [SerializeField][Range(4, 32)] private byte _renderDistance;
        [SerializeField] private NoiseSettings _terrainNoiseSettings;

        public override void InstallBindings()
        {
            BindWorldGenerationSettings();
            BindGenerators();
        }

        private void BindWorldGenerationSettings()
        {
            Container
                .Bind<WorldGenerationSettings>()
                .AsSingle()
                .WithArguments(_chunkLength, _chunkHeight, _renderDistance);
        }

        private void BindGenerators()
        {
            Container
                .Bind<TerrainGenerator>()
                .AsSingle()
                .WithArguments(_terrainNoiseSettings);
        }
    }
}