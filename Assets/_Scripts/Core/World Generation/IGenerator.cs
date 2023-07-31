using HerosJourney.Core.WorldGeneration.Chunks;

namespace HerosJourney.Core.WorldGeneration
{
    public interface IGenerator
    {
        void Generate(ChunkData chunkData);        
    }
}