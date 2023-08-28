using HerosJourney.Core.WorldGeneration;
using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Noises;
using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class WorldInstaller : MonoInstaller
    {
        [SerializeField] private NoiseSettings _terrainNoiseSettings;
        [SerializeField] private VoxelData _air;
        [SerializeField] private VoxelData _dirt;
        [SerializeField] private VoxelData _grass;
        [SerializeField] private byte _chunkLength;
        [SerializeField] private byte _chunkHeight;

        public override void InstallBindings()
        {
            BindWorldData();
            BindGenerators();
            BindMeshDataBuilder();
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
                .BindInterfacesAndSelfTo<TerrainGenerator>()
                .AsSingle()
                .WithArguments(_terrainNoiseSettings, _air.id, _dirt.id, _grass.id);
        }

        private void BindMeshDataBuilder()
        {
            Container
                .BindInterfacesAndSelfTo<MeshDataBuilder>()
                .AsSingle();
        }
    }
}