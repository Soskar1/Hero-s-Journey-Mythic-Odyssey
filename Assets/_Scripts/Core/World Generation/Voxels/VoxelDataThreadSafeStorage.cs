using HerosJourney.Utils;
using System.Collections.Generic;
using Zenject;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    public class VoxelDataThreadSafeStorage : ThreadSafeStorage<int, ThreadSafeVoxelData>, IInitializable
    {
        private List<VoxelData> _voxelData;

        public VoxelDataThreadSafeStorage(List<VoxelData> voxelData) => _voxelData = voxelData;

        public void Initialize()
        {
            Initialize(_voxelData.Count);

            foreach (var voxelData in _voxelData)
                Add(voxelData.id, voxelData);
        }
    }
}