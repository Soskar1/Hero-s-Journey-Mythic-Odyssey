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
            if (localPosition.y < _waterThreshold && chunkData.voxels[localPosition.x, localPosition.y, localPosition.z].VoxelType == VoxelType.Air)
            {
                ChunkDataHandler.SetVoxelAt(chunkData, new Voxels.Voxel(VoxelData, chunkData.WorldPosition + localPosition), localPosition);

                VoxelType voxelType = ChunkDataHandler.GetVoxelAt(chunkData, localPosition + Vector3Int.down).VoxelType;
                if (voxelType == VoxelType.Solid)
                    ChunkDataHandler.SetVoxelAt(chunkData, new Voxels.Voxel(_sand, chunkData.WorldPosition + localPosition), localPosition + Vector3Int.down);

                return true;
            }

            return false;
        }
    }
}