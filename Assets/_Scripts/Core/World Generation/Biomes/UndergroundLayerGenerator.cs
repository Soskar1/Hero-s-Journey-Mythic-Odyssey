using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Terrain
{
    public class UndergroundLayerGenerator : LayerGenerator
    {
        protected override bool TryGenerateVoxels(ChunkData chunkData, Vector3Int localPosition, int surfaceHeightNoise)
        {
            if (localPosition.y < surfaceHeightNoise)
            {
                ChunkDataHandler.SetVoxelAt(chunkData, MainVoxel, localPosition);
                return true;
            }

            return false;
        }
    }
}