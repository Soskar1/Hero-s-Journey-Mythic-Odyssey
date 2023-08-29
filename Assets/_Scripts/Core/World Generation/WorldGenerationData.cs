using System.Collections.Generic;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration
{
    public class WorldGenerationData
    {
        public List<int3> chunkDataToCreate;
        public List<int3> chunkRenderersToCreate;
    }
}