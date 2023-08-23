using System.Collections.Generic;
using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Noise;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Terrain
{
    public class TerrainGenerator
    {
        private readonly TSNoiseSettings _noiseSettings;
        private readonly WorldGenerationSettings _worldGenerationSettings;

        public TerrainGenerator(NoiseSettings noiseSettings, WorldGenerationSettings worldGenerationSettings)
        {
            _noiseSettings = new TSNoiseSettings(noiseSettings);
            _worldGenerationSettings = worldGenerationSettings;
        }

        public Dictionary<int3, ChunkData> Generate(NativeList<int3> chunkPositionsToCreate)
        {
            Dictionary<int3, ChunkData> generatedChunks = new Dictionary<int3, ChunkData>();
            NativeArray<ushort> generatedVoxelsID = new NativeArray<ushort>(_worldGenerationSettings.ChunkSize, Allocator.Persistent);
            
            foreach (var position in chunkPositionsToCreate)
            {
                GenerateTerrainJob job = new GenerateTerrainJob
                {
                    worldPosition = position,
                    generatedVoxelsID = generatedVoxelsID,
                    noiseSettings = _noiseSettings,
                    chunkHeight = _worldGenerationSettings.WorldData.chunkHeight,
                    chunkLength = _worldGenerationSettings.WorldData.chunkLength,
                    airID = 0,
                    surfaceID = 1,
                    undergroundID = 2
                };

                JobHandle jobHandle = job.Schedule();
                jobHandle.Complete();

                ChunkData chunkData = new ChunkData(_worldGenerationSettings.WorldData);
                generatedVoxelsID.CopyTo(chunkData.voxels);

                generatedChunks.Add(position, chunkData);
            }

            generatedVoxelsID.Dispose();

            return generatedChunks;
        }
    }

    [BurstCompile]
    public struct GenerateTerrainJob : IJob
    {
        public NativeArray<ushort> generatedVoxelsID;
        public TSNoiseSettings noiseSettings;
        public int3 worldPosition;
        public byte chunkLength;
        public byte chunkHeight;
        public ushort airID;
        public ushort surfaceID;
        public ushort undergroundID;
        
        public void Execute()
        {
            for (int x = 0; x < chunkLength; ++x)
            {
                for (int z = 0; z < chunkLength; ++z)
                {
                    float noiseValue = OctavePerlinNoise(worldPosition.x + x, worldPosition.z + z);
                    int groundHeight = Mathf.RoundToInt(noiseValue * chunkHeight);

                    for (int localY = worldPosition.y; localY < worldPosition.y + chunkHeight; ++localY)
                        GenerateVoxel(new int3(x, localY, z), groundHeight);
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
                total += noise.cnoise(new float2((x + noiseSettings.offset.x) * frequency, (y + noiseSettings.offset.y) * frequency)) * amplitude;

                maxValue += amplitude;

                amplitude *= noiseSettings.persistence;
                frequency *= 2;
            }

            return total / maxValue;
        }

        private void GenerateVoxel(int3 localPosition, int surfaceHeightNoise)
        {
            if (GenerateAir(localPosition, surfaceHeightNoise)
                || GenerateSurface(localPosition, surfaceHeightNoise)
                || GenerateUnderground(localPosition, surfaceHeightNoise))
                return;
        }

        private bool GenerateAir(int3 localPosition, int surfaceHeightNoise)
        {
            if (localPosition.y > surfaceHeightNoise)
            {
                SetVoxelAt(airID, localPosition);
                return true;
            }

            return false;
        }

        private bool GenerateSurface(int3 localPosition, int surfaceHeightNoise)
        {
            if (localPosition.y == surfaceHeightNoise)
            {
                SetVoxelAt(surfaceID, localPosition);
                return true;
            }

            return false;
        }

        private bool GenerateUnderground(int3 localPosition, int surfaceHeightNoise)
        {
            if (localPosition.y < surfaceHeightNoise)
            {
                SetVoxelAt(undergroundID, localPosition);
                return true;
            }

            return false;
        }

        private bool GenerateWater()
        {
            return false;
        }

        private void SetVoxelAt(ushort voxelID, int3 localPosition)
        {
            int index = ChunkHandler.LocalPositionToIndex(chunkLength, chunkHeight, localPosition);
            generatedVoxelsID[index] = voxelID;
        }
    }
}