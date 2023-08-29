using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Noises;
using HerosJourney.Core.WorldGeneration.Voxels;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration
{
    public class TerrainGenerator : IInitializable
    {
        private readonly WorldData _worldData;
        private readonly ThreadSafeNoiseSettings _noiseSettings;
        private readonly ushort _airID;
        private readonly ushort _dirtID;
        private readonly ushort _grassID;

        private Noise _noise;

        public TerrainGenerator(WorldData worldData, NoiseSettings noiseSettings, ushort airID, ushort dirtID, ushort grassID)
        {
            _worldData = worldData;
            _noiseSettings = noiseSettings;
            _airID = airID;
            _dirtID = dirtID;
            _grassID = grassID;
        }

        public void Initialize() => _noise = new Noise(_noiseSettings);

        public void Generate(List<int3> chunkDataPositionsToCreate)
        {
            foreach (var position in chunkDataPositionsToCreate)
            {
                ChunkData chunkData = new ChunkData(_worldData, position);

                for (int x = 0; x < chunkData.Length; ++x)
                {
                    for (int z = 0; z < chunkData.Length; ++z)
                    {
                        float noiseValue = _noise.OctavePerlinNoise(chunkData.WorldPosition.x + x, chunkData.WorldPosition.z + z);
                        float groundHeight = Mathf.FloorToInt(noiseValue * chunkData.Height);

                        for (int y = 0; y < chunkData.Height; ++y)
                        {
                            int3 localPosition = new int3(x, y, z);

                            if (y < groundHeight)
                                chunkData.Voxels[VoxelExtensions.GetVoxelIndex(localPosition)] = _dirtID;
                            else if (y == groundHeight)
                                chunkData.Voxels[VoxelExtensions.GetVoxelIndex(localPosition)] = _grassID;
                            else
                                chunkData.Voxels[VoxelExtensions.GetVoxelIndex(localPosition)] = _airID;
                        }
                    }
                }

                _worldData.ExistingChunks.Add(position, chunkData);
            }
        }
    }
}