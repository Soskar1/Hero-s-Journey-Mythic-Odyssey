using HerosJourney.Core.WorldGeneration.Voxels;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace HerosJourney.GlobalInstallers
{
    public class VoxelInstaller : MonoInstaller
    {
        [SerializeField] private List<VoxelData> _voxelData;

        public override void InstallBindings()
        {
            Container
                .BindInterfacesAndSelfTo<VoxelDataStorage>()
                .AsSingle()
                .WithArguments(_voxelData)
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<VoxelStorage>()
                .AsSingle()
                .NonLazy();
        }
    }
}
