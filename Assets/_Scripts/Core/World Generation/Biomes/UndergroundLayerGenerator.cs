using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Biomes
{
    public class UndergroundLayerGenerator : LayerGenerator
    {
        protected override bool TryGenerateVoxels(ChunkData chunkData, Vector3Int localPosition, int surfaceHeightNoise)
        {
            if (localPosition.y < surfaceHeightNoise)
            {
                ChunkDataHandler.SetVoxelAt(chunkData, new Voxels.Voxel(VoxelData, chunkData.WorldPosition + localPosition), localPosition);
                return true;
            }

            return false;
        }
    }
}