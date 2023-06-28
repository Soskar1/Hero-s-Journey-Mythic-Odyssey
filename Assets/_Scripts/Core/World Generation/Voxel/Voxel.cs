namespace HerosJourney.Core.WorldGeneration.Voxels
{
    public class Voxel
    {
        public VoxelData data;
        public VoxelType VoxelType => data.type;

        public Voxel(VoxelData data)
        {
            this.data = data;
        }
    }
}