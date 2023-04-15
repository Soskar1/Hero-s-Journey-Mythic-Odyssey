using HerosJourney.Core.WorldGeneration.Biomes;
using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private BiomeGenerator _biomeGenerator;

        public void GenerateChunkData(ref ChunkData data)
        {
            for (int x = 0; x < data.ChunkSize; ++x)
                for (int z = 0; z < data.ChunkSize; ++z)
                    _biomeGenerator.GenerateChunk(ref data, x, z);
        }
    }
}