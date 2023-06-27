using System.Collections.Generic;
using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;
using System.Linq;

namespace HerosJourney.Core.WorldGeneration.Biomes
{
    public class SandLayerGenerator : LayerGenerator
    {
        protected override bool TryGenerateVoxels(ChunkData chunkData, Vector3Int localPosition, int surfaceHeightNoise)
        {
            

            return false;
        }
    }
}