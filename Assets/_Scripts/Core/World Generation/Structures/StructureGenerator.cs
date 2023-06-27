using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Noises;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Structures
{
    public class StructureGenerator : MonoBehaviour
    {
        [SerializeField] private NoiseSettings _noiseSettings;
        [SerializeField] private StructurePlacementSettings _structurePlacementSettings;

        public StructureData GenerateStructureData(ChunkData chunkData)
        {
            StructureData structureData = new StructureData();
            float[,] noise = GenerateNoise(chunkData);

            structureData.structurePositions = StructurePlacement.PlaceStructures(noise, chunkData.WorldPosition.x, chunkData.WorldPosition.z, _structurePlacementSettings);
            
            return structureData;
        }

        private float[,] GenerateNoise(ChunkData chunkData)
        {
            float[,] noise = new float[chunkData.ChunkLength, chunkData.ChunkLength];

            int xStart = chunkData.WorldPosition.x;
            int xEnd = chunkData.WorldPosition.x + chunkData.ChunkLength;
            int zStart = chunkData.WorldPosition.z;
            int zEnd = chunkData.WorldPosition.z + chunkData.ChunkLength;

            int xIndex = 0;
            int zIndex = 0;
            for (int x = xStart; x < xEnd; ++x)
            {
                for (int z = zStart; z < zEnd; ++z)
                {
                    noise[xIndex, zIndex] = Noise.OctavePerlinNoise(x, z, _noiseSettings);

                    ++zIndex;
                }
                ++xIndex;
                zIndex = 0;
            }

            return noise;
        }
    }
}