using HerosJourney.Utils;
using System.Collections.Generic;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    public class VoxelDataTSStorage : TSStorage<int, TSVoxelData>
    {
        private List<VoxelData> _voxelData;

        public VoxelDataTSStorage(List<VoxelData> voxelData) => _voxelData = voxelData;

        public override void Initialize()
        {
            base.Initialize();

            foreach (var voxelData in _voxelData)
                Add(voxelData.id, new TSVoxelData(voxelData));
        }
    }
}