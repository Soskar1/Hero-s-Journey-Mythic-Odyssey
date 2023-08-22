using HerosJourney.Utils;
using System.Collections.Generic;
using Zenject;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    public class VoxelDataTSStorage : TSStorage<int, TSVoxelData>, IInitializable
    {
        private List<VoxelData> _voxelData;

        public VoxelDataTSStorage(List<VoxelData> voxelData) => _voxelData = voxelData;

        public void Initialize()
        {
            Initialize(_voxelData.Count);

            foreach (var voxelData in _voxelData)
                Add(voxelData.id, new TSVoxelData(voxelData));
        }
    }
}