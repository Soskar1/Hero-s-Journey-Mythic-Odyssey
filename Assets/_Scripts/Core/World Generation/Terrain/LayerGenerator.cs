using HerosJourney.Core.WorldGeneration.Chunks;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Terrain
{
    public abstract class LayerGenerator
    {
        private LayerGenerator _next;
        protected ushort _voxelID;

        public bool TryGenerateLayer(ChunkData chunkData, int3 localPosition, int surfaceHeightNoise)
        {
            if (TryGenerateVoxels(chunkData, localPosition, surfaceHeightNoise))
                return true;

            if (_next != null)
                _next.TryGenerateLayer(chunkData, localPosition, surfaceHeightNoise);

            return false;
        }

        protected abstract bool TryGenerateVoxels(ChunkData chunkData, int3 localPosition, int surfaceHeightNoise);
    }
}