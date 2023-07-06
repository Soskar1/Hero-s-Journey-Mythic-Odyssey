using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Terrain
{
    public class WaterLayerGenerator : LayerGenerator
    {
        [SerializeField] private VoxelData _sand;
        [SerializeField] private int _waterThreshold;

        protected override bool TryGenerateVoxels(ChunkData chunkData, Vector3Int localPosition, int surfaceHeightNoise)
        {
            if (localPosition.y < _waterThreshold && chunkData.voxels[localPosition.x, localPosition.y, localPosition.z].VoxelType == VoxelType.Air)
            {
                ChunkDataHandler.SetVoxelAt(chunkData, MainVoxel, localPosition);

                if (localPosition.y == surfaceHeightNoise + 1)
                {
                    localPosition.y = surfaceHeightNoise;
                    ChunkDataHandler.SetVoxelAt(chunkData, VoxelStorage.Get(_sand), localPosition);
                }

                return true;
            }

            return false;
        }
    }
}