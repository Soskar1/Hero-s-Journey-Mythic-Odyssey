using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Biomes
{
    public class AirLayerGenerator : LayerGenerator
    {
        protected override bool TryGenerateVoxels(ChunkData chunkData, Vector3Int position, int surfaceHeightNoise)
        {
            if (position.y > surfaceHeightNoise)
            {
                ChunkDataHandler.SetVoxelAt(chunkData, Voxel, position);
                return true;
            }

            return false;
        }
    }
}