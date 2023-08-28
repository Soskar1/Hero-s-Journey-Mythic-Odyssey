using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Voxels;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class TerrainGenerator
    {
        private readonly WorldData _worldData;
        private readonly ushort _airID;
        private readonly ushort _dirtID;
        private readonly ushort _grassID;

        public TerrainGenerator(WorldData worldData, ushort airID, ushort dirtID, ushort grassID)
        {
            _worldData = worldData;
            _airID = airID;
            _dirtID = dirtID;
            _grassID = grassID;
        }

        public List<ChunkData> Generate(List<int3> chunkDataPositionsToCreate)
        {
            List<ChunkData> generatedChunkData = new List<ChunkData>();

            foreach (var position in chunkDataPositionsToCreate)
            {
                ChunkData chunkData = new ChunkData(_worldData, position);

                for (int x = 0; x < chunkData.Length; ++x)
                {
                    for (int z = 0; z < chunkData.Length; ++z)
                    {
                        var groundHeight = Mathf.FloorToInt(Mathf.PerlinNoise((chunkData.WorldPosition.x + x) * 0.005f, (chunkData.WorldPosition.z + z) * 0.005f) * chunkData.Height);

                        for (int i = 0; i < chunkData.Height; ++i)
                        {
                            if (i < groundHeight)
                                chunkData.Voxels[VoxelExtensions.GetVoxelIndex(new int3(x, i, z))] = _dirtID;
                            else if (i == groundHeight)
                                chunkData.Voxels[VoxelExtensions.GetVoxelIndex(new int3(x, i, z))] = _grassID;
                            else
                                chunkData.Voxels[VoxelExtensions.GetVoxelIndex(new int3(x, i, z))] = _airID;
                        }
                    }
                }

                generatedChunkData.Add(chunkData);
            }

            return generatedChunkData;
        }
    }
}