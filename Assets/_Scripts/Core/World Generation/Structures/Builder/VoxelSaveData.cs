using HerosJourney.Utils;
using System;

namespace HerosJourney.Core.WorldGeneration.Structures.Builder
{
    [Serializable]
    public struct VoxelSaveData
    {
        public LightVector3Int position;
        public int id;
    }
}
