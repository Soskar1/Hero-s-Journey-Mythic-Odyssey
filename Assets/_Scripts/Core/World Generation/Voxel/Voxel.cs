using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class Voxel
    {
        //public VoxelData data;
        public VoxelType type;

        public Voxel(VoxelType type)
        {
            this.type = type;
        }
    }
}