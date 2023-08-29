using System.Collections.Generic;
using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class VoxelDataInstaller : MonoInstaller
    {
        [SerializeField] private List<VoxelData> _voxelData;

        public override void InstallBindings()
        {
            BindVoxelData();
        }

        private void BindVoxelData()
        {
            Container
                .BindInterfacesAndSelfTo<VoxelDataThreadSafeStorage>()
                .AsSingle()
                .WithArguments(_voxelData);
        }
    }
}