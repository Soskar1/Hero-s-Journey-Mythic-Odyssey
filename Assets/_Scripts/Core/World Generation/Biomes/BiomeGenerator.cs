using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Noises;
using HerosJourney.Core.WorldGeneration.Structures;
using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace HerosJourney.Core.WorldGeneration.Biomes
{
    public class BiomeGenerator : MonoBehaviour
    {
        [SerializeField] private List<LayerGenerator> _layerGenerators;
        [SerializeField] private NoiseSettings _noiseSettings;
        [SerializeField] private StructureGenerator _structureGenerator;

        private World _world;

        [Inject]
        private void Construct(World world) => _world = world;

        public void GenerateChunkColumn(ChunkData chunkData, int x, int z)
        {
            float noise = Noise.OctavePerlinNoise(x + chunkData.WorldPosition.x, z + chunkData.WorldPosition.z, _noiseSettings);
            int groundPosition = Mathf.RoundToInt(noise * _world.ChunkHeight);

            foreach (LayerGenerator layerGenerator in _layerGenerators)
                for (int localY = chunkData.WorldPosition.y; localY < chunkData.WorldPosition.y + chunkData.ChunkHeight; ++localY)
                    layerGenerator.TryGenerateLayer(chunkData, new Vector3Int(x, localY, z), groundPosition);
        }

        public StructureData GenerateStructureData(ChunkData chunkData) => _structureGenerator.GenerateStructureData(chunkData);
    }
}