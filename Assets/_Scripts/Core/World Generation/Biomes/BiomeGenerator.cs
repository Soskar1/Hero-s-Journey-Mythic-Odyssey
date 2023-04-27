using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Noises;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Biomes
{
    public class BiomeGenerator : MonoBehaviour
    {
        [SerializeField] private LayerGenerator _startingLayer;
        [SerializeField] private NoiseSettings _noiseSettings;

        public ChunkData GenerateChunkColumn(ChunkData chunkData, int x, int z)
        {
            float noise = Noise.OctavePerlinNoise(x + chunkData.WorldPosition.x, z + chunkData.WorldPosition.z, _noiseSettings);
            int groundPosition = Mathf.RoundToInt(noise * chunkData.ChunkHeight);

            for (int y = chunkData.WorldPosition.y; y < chunkData.WorldPosition.y + chunkData.ChunkHeight; ++y)
                _startingLayer.TryGenerateLayer(chunkData, new Vector3Int(x, y, z), groundPosition);

            return chunkData;
        }
    }
}