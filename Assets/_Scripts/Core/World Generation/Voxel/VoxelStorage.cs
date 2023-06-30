using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    public class VoxelStorage : MonoBehaviour
    {
        [SerializeField] private List<VoxelData> _voxelData;
        private Dictionary<VoxelData, Voxel> _voxelInstances = new Dictionary<VoxelData, Voxel>();

        private void Awake()
        {
            foreach (VoxelData voxelData in _voxelData)
            {
                Voxel voxel = new Voxel(voxelData);
                _voxelInstances.Add(voxelData, voxel);
            }
        }

        public Voxel GetVoxel(VoxelData voxelData) => _voxelInstances[voxelData];
    }
}