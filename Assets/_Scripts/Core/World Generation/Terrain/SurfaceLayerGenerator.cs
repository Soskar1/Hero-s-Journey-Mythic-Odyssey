using HerosJourney.Core.WorldGeneration.Chunks;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Terrain
{
    public class SurfaceLayerGenerator : LayerGenerator
    {
        protected override bool TryGenerateVoxels(ChunkData chunkData, int3 localPosition, int surfaceHeightNoise)
        {
            if (localPosition.y == surfaceHeightNoise)
            {
                ChunkHandler.SetVoxelAt(chunkData, _voxelID, localPosition);
                return true;
            }

            return false;
        }
    }
}
