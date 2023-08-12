using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration.Terrain
{
    public class WaterLayerGenerator : LayerGenerator
    {
        [SerializeField] private VoxelData _sand;
        [SerializeField] private int _waterThreshold;

        private VoxelDataStorage _voxelDataStorage;

        [Inject]
        private void Construct(VoxelDataStorage voxelDataStorage) => _voxelDataStorage = voxelDataStorage;

        protected override bool TryGenerateVoxels(ChunkData chunkData, Vector3Int localPosition, int surfaceHeightNoise)
        {
            if (localPosition.y < _waterThreshold && _voxelDataStorage.GetVoxelType(chunkData.voxelId[localPosition.x, localPosition.y, localPosition.z]) == VoxelType.Air)
            {
                ChunkDataHandler.SetVoxelAt(chunkData, MainVoxelId, localPosition);

                if (localPosition.y == surfaceHeightNoise + 1)
                {
                    localPosition.y = surfaceHeightNoise;
                    ChunkDataHandler.SetVoxelAt(chunkData, _sand.id, localPosition);
                }

                return true;
            }

            return false;
        }
    }
}