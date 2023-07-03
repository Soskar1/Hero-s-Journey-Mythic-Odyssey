using HerosJourney.Utils;
using Zenject;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    public class VoxelStorage : Storage<VoxelData, Voxel>, IInitializable
    {
        private VoxelDataStorage _voxelDataStorage;

        public VoxelStorage(VoxelDataStorage voxelDataStorage) => _voxelDataStorage = voxelDataStorage;

        public void Initialize()
        {
            foreach (VoxelData voxelData in _voxelDataStorage.GetValues())
            {
                Voxel voxel = new Voxel(voxelData);
                Add(voxelData, voxel);
            }
        }

        public Voxel GetVoxelByID(int id) => Get(_voxelDataStorage.Get(id));
    }
}