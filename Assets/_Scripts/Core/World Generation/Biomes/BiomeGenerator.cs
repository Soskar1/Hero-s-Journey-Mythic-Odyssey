using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Noises;
using UnityEngine;
using System.Collections.Generic;

namespace HerosJourney.Core.WorldGeneration.Biomes
{
    public class BiomeGenerator : MonoBehaviour
    {
        [SerializeField] private LayerGenerator _startingLayerGenerator;
        [SerializeField] private NoiseSettings _noiseSettings;

        [SerializeField] private List<LayerGenerator> _additionalLayerGenerators;

        public ChunkData GenerateChunkColumn(ChunkData chunkData, int x, int z)
        {
            float noise = Noise.OctavePerlinNoise(x + chunkData.WorldPosition.x, z + chunkData.WorldPosition.z, _noiseSettings);
            int groundPosition = Mathf.RoundToInt(noise * chunkData.ChunkHeight);

            for (int y = chunkData.WorldPosition.y; y < chunkData.WorldPosition.y + chunkData.ChunkHeight; ++y)
                _startingLayerGenerator.TryGenerateLayer(chunkData, new Vector3Int(x, y, z), groundPosition);

            foreach (LayerGenerator layerGenerator in _additionalLayerGenerators)
                for (int y = chunkData.WorldPosition.y; y < chunkData.WorldPosition.y + chunkData.ChunkHeight; ++y)
                    layerGenerator.TryGenerateLayer(chunkData, new Vector3Int(x, y, z), groundPosition);

            return chunkData;
        }
    }
}