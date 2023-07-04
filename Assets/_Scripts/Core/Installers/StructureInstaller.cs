using HerosJourney.Core.WorldGeneration.Structures;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class StructureInstaller : MonoInstaller
    {
        [SerializeField] private List<TextAsset> _structureFiles = new List<TextAsset>();

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<StructureStorage>()
                .AsSingle()
                .WithArguments(_structureFiles)
                .NonLazy();
        }
    }
}