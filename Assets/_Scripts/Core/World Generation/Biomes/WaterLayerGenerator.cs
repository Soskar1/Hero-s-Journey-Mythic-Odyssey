using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Biomes
{
    public class WaterLayerGenerator : LayerGenerator
    {
        [SerializeField] private VoxelData _sand;
        [SerializeField] private int _waterThreshold;

        protected override bool TryGenerateVoxels(ChunkData chunkData, Vector3Int localPosition, int surfaceHeightNoise)
        {
            if (localPosition.y < _waterThreshold && chunkData.voxels[localPosition.x, localPosition.y, localPosition.z].GetVoxelType() == VoxelType.Air)
            {
                ChunkDataHandler.SetVoxelAt(chunkData, Voxel, localPosition);

                VoxelType voxelType = ChunkDataHandler.GetVoxelAt(chunkData, localPosition + Vector3Int.down).GetVoxelType();
                if (voxelType == VoxelType.Solid)
                    ChunkDataHandler.SetVoxelAt(chunkData, new Voxel(_sand), localPosition + Vector3Int.down);

                return true;
            }

            return false;
        }
    }
}