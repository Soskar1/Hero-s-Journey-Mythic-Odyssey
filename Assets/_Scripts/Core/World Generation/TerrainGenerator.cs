using HerosJourney.Core.WorldGeneration.Biomes;
using HerosJourney.Core.WorldGeneration.Chunks;

namespace HerosJourney.Core.WorldGeneration
{
    public class TerrainGenerator
    {
        private BiomeGenerator _biomeGenerator;

        public TerrainGenerator(BiomeGenerator biomeGenerator) => _biomeGenerator = biomeGenerator;

        public void GenerateChunkData(ChunkData chunkData)
        {
            for (int x = 0; x < chunkData.ChunkLength; ++x)
                for (int z = 0; z < chunkData.ChunkLength; ++z)
                    _biomeGenerator.GenerateChunkColumn(chunkData, x, z);
        }
    }
}