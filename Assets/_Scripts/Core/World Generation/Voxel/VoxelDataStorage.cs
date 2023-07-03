using HerosJourney.Utils;
using System.Collections.Generic;
using Zenject;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    public class VoxelDataStorage : Storage<int, VoxelData>, IInitializable
    {
        private List<VoxelData> _voxelData;

        public VoxelDataStorage(List<VoxelData> voxelData) => _voxelData = voxelData;

        public void Initialize()
        {
            foreach (var voxelData in _voxelData)
                Add(voxelData.id, voxelData);
        }
    }
}