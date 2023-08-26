using HerosJourney.Core.WorldGeneration;
using System.Collections;
using System.Collections.Generic;
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
        }

        private void BindWorldData()
        {
            Container
                .Bind<WorldData>()
                .AsSingle()
                .WithArguments(_chunkLength, _chunkHeight);
        }
    }
}