using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class VoxelInstaller : MonoInstaller
    {
        [SerializeField] private VoxelDataStorage _voxelStorage;

        public override void InstallBindings()
        {
            Container
                .Bind<VoxelDataStorage>()
                .FromInstance(_voxelStorage)
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<VoxelStorage>()
                .AsSingle()
                .NonLazy();
        }
    }
}
