using HerosJourney.Utils;
using System;

namespace HerosJourney.StructureBuilder.Saving
{
    [Serializable]
    public struct VoxelSaveData
    {
        public LightVector3Int pos;
        public int id;
    }
}
