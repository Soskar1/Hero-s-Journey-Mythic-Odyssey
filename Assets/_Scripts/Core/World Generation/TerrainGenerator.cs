using Unity.Mathematics;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class TerrainGenerator
    {
        public void Generate(ChunkData chunkData)
        {
            for (int x = 0; x < chunkData.Length; ++x)
            {
                for (int z = 0; z < chunkData.Length; ++z)
                {
                    var y = Mathf.FloorToInt(Mathf.PerlinNoise((chunkData.WorldPosition.x + x) * 0.15f, (chunkData.WorldPosition.z + z) * 0.15f) * chunkData.Height);

                    for (int i = 0; i < y; ++i)
                        chunkData.Voxels[VoxelExtensions.GetVoxelIndex(new int3(x, i, z))] = VoxelType.Stone;

                    for (int i = y; i < chunkData.Height; ++i)
                        chunkData.Voxels[VoxelExtensions.GetVoxelIndex(new int3(x, i, z))] = VoxelType.Air;
                }
            }
        }
    }
}