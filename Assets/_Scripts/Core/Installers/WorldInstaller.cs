using HerosJourney.Core.WorldGeneration;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class WorldInstaller : MonoInstaller
    {
        [SerializeField] private byte _chunkLength;
        [SerializeField] private byte _chunkHeight;
        [SerializeField][Range(4, 32)] private byte _renderDistance;

        public override void InstallBindings()
        {
            BindWorldGenerationSettings();
        }

        private void BindWorldGenerationSettings()
        {
            Container
                .Bind<WorldGenerationSettings>()
                .AsSingle()
                .WithArguments(_chunkLength, _chunkHeight, _renderDistance);
        }
    }
}