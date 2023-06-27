using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    public class Voxel
    {
        public VoxelData data;
        private Vector3Int _worldPosition;

        public Vector3Int Position => _worldPosition;
        public VoxelType VoxelType => data.type;

        public Voxel(VoxelData data, Vector3Int worldPosition)
        {
            this.data = data;
            _worldPosition = worldPosition;
        }
    }
}