using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    public class VoxelStorage : IInitializable
    {
        private VoxelDataStorage _voxelDataStorage;
        private Dictionary<VoxelData, Voxel> _voxelInstances = new Dictionary<VoxelData, Voxel>();

        public VoxelStorage(VoxelDataStorage voxelDataStorage) => _voxelDataStorage = voxelDataStorage;

        public void Initialize()
        {
            foreach (VoxelData voxelData in _voxelDataStorage.GetAllData())
            {
                Voxel voxel = new Voxel(voxelData);
                _voxelInstances.Add(voxelData, voxel);
            }
        }

        public Voxel GetVoxel(VoxelData voxelData)
        {
            if (_voxelInstances.ContainsKey(voxelData))
                return _voxelInstances[voxelData];

            Debug.LogError($"VoxelData {voxelData} does not exist");
            return null;
        }

        public Voxel GetVoxelByID(int id) => _voxelInstances[_voxelDataStorage.FindVoxelData(id)];
    }
}