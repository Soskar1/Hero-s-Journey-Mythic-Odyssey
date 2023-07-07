using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;
using Zenject;

namespace HerosJourney.StructureBuilder.Installers
{
    public class BuilderInstaller : MonoInstaller
    {
        [SerializeField] private StructureRenderer _structureRenderer;
        [SerializeField] private Vector3Int _size;
        [SerializeField] private VoxelData _groundVoxel;

        public override void InstallBindings()
        {
            BindStructure();            
            BindBuilder();
        }

        private void BindStructure()
        {
            Container
                .Bind<StructureData>()
                .AsSingle()
                .WithArguments(_size)
                .NonLazy();

            Container
                .Bind<StructureRenderer>()
                .FromInstance(_structureRenderer)
                .AsSingle();
        }

        private void BindBuilder()
        {
            Container
                .Bind<VoxelPlacement>()
                .AsSingle();

            Container
                .BindInterfacesAndSelfTo<StructureBuilder>()
                .AsSingle()
                .WithArguments(_groundVoxel.id)
                .NonLazy();
        }
    }
}