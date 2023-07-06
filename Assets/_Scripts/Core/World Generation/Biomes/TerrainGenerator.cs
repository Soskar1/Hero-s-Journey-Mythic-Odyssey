using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Noises;
using UnityEngine;
using Zenject;
using System.Collections.Generic;

namespace HerosJourney.Core.WorldGeneration.Terrain
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private List<LayerGenerator> _layerGenerators;
        [SerializeField] private NoiseSettings _noiseSettings;

        private World _world;

        [Inject]
        private void Construct(World world) => _world = world;

        public void GenerateTerrain(ChunkData chunkData)
        {
            Noise.SetSeed(_world.WorldData.worldSeed);

            for (int x = 0; x < chunkData.ChunkLength; ++x)
                for (int z = 0; z < chunkData.ChunkLength; ++z)
                    GenerateChunkColumn(chunkData, x, z);
        }

        private void GenerateChunkColumn(ChunkData chunkData, int x, int z)
        {
            GenerateGroundHeight(chunkData, x, z);

            foreach (LayerGenerator layerGenerator in _layerGenerators)
                for (int localY = chunkData.WorldPosition.y; localY < chunkData.WorldPosition.y + chunkData.ChunkHeight; ++localY)
                    layerGenerator.TryGenerateLayer(chunkData, new Vector3Int(x, localY, z), chunkData.groundHeight[x, z]);
        }

        private void GenerateGroundHeight(ChunkData chunkData, int x, int z)
        {
            float noise = Noise.OctavePerlinNoise(x + chunkData.WorldPosition.x, z + chunkData.WorldPosition.z, _noiseSettings);
            chunkData.groundHeight[x, z] = Mathf.RoundToInt(noise * _world.ChunkHeight);
        }
    }
}