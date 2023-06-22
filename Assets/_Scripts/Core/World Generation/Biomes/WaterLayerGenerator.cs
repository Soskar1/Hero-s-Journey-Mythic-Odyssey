using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Biomes
{
    public class WaterLayerGenerator : LayerGenerator
    {
        [SerializeField] private int _waterThreshold;

        protected override bool TryGenerateVoxels(ChunkData chunkData, Vector3Int position, int surfaceHeightNoise)
        {
            if (position.y < _waterThreshold && chunkData.voxels[position.x, position.y, position.z].GetVoxelType() == Voxels.VoxelType.Air)
            {
                ChunkDataHandler.SetVoxelAt(chunkData, Voxel, position);
                return true;
            }

            return false;
        }
    }
}