using HerosJourney.Core.WorldGeneration.Biomes;
using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class TerrainGenerator : MonoBehaviour
    {
        [SerializeField] private BiomeGenerator _biomeGenerator;

        public ChunkData GenerateTerrain(ChunkData data, Vector2Int offset)
        {
            for (int x = 0; x < data.ChunkSize; ++x)
                for (int z = 0; z < data.ChunkSize; ++z)
                    data = _biomeGenerator.GenerateChunk(data, x, z, offset);

            return data;
        }
    }
}