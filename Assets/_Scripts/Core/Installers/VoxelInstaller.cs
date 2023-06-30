using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class VoxelInstaller : MonoInstaller
    {
        [SerializeField] private VoxelStorage _voxelStorage;

        public override void InstallBindings()
        {
            Container
                .Bind<VoxelStorage>()
                .FromInstance(_voxelStorage)
                .AsSingle();
        }
    }
}
