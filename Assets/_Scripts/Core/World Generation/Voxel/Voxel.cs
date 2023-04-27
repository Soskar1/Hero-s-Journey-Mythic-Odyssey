namespace HerosJourney.Core.WorldGeneration.Voxels
{
    public class Voxel
    {
        public VoxelData data;

        public Voxel(VoxelData data)
        {
            this.data = data;
        }

        new public VoxelType GetType() => data.type;
    }
}