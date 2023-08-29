using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Noises;
using HerosJourney.Core.WorldGeneration.Voxels;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    [BurstCompile]
    public struct TerrainGenerationJob : IJob
    {
        public ThreadSafeChunkData chunkData;
        [ReadOnly] public ThreadSafeNoiseSettings noiseSettings;
        [ReadOnly] public ushort dirtID;
        [ReadOnly] public ushort grassID;
        [ReadOnly] public ushort airID;

        public void Execute()
        {
            for (int x = 0; x < chunkData.Length; ++x)
            {
                for (int z = 0; z < chunkData.Length; ++z)
                {
                    float noiseValue = OctavePerlinNoise(chunkData.WorldPosition.x + x, chunkData.WorldPosition.z + z);
                    float groundHeight = Mathf.FloorToInt(noiseValue * chunkData.Height);

                    for (int y = 0; y < chunkData.Height; ++y)
                    {
                        int3 localPosition = new int3(x, y, z);
                        int index = VoxelExtensions.GetVoxelIndex(localPosition);

                        if (y < groundHeight)
                            chunkData.voxels[index] = dirtID;
                        else if (y == groundHeight)
                            chunkData.voxels[index] = grassID;
                        else
                            chunkData.voxels[index] = airID;
                    }
                }
            }
        }

        private float OctavePerlinNoise(float x, float y)
        {
            x *= noiseSettings.noiseZoom;
            y *= noiseSettings.noiseZoom;

            float total = 0;
            float maxValue = 0;

            float amplitude = 1;
            float frequency = 1;

            for (int i = 0; i < noiseSettings.octaves; ++i)
            {
                total += Mathf.PerlinNoise((x + noiseSettings.offset.x) * frequency, (y + noiseSettings.offset.y) * frequency) * amplitude;

                maxValue += amplitude;

                amplitude *= noiseSettings.persistence;
                frequency *= 2;
            }

            return total / maxValue;
        }
    }
}