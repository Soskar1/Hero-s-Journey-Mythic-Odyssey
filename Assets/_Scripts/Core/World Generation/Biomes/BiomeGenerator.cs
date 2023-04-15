using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Noises;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Biomes
{
    public class BiomeGenerator : MonoBehaviour
    {
        [SerializeField] private LayerGenerator _startingLayer;
        [SerializeField] private NoiseSettings _noiseSettings;

        public ChunkData GenerateChunk(ref ChunkData data, int x, int z)
        {
            float noise = Noise.OctavePerlinNoise(x + data.WorldPosition.x, z + data.WorldPosition.z, _noiseSettings);
            int groundPosition = Mathf.RoundToInt(noise * data.ChunkHeight);

            for (int y = data.WorldPosition.y; y < data.WorldPosition.y + data.ChunkHeight; ++y)
                _startingLayer.TryGenerateLayer(data, new Vector3Int(x, y, z), groundPosition);

            return data;
        }
    }
}