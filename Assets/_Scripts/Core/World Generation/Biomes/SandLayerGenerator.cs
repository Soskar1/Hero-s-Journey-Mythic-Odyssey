using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Biomes
{
    public class SandLayerGenerator : LayerGenerator
    {
        protected override bool TryGenerateVoxels(ChunkData chunkData, Vector3Int position, int surfaceHeightNoise)
        {


            return false;
        }
    }
}