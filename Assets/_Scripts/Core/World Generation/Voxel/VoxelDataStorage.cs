using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    public class VoxelDataStorage : MonoBehaviour
    {
        [SerializeField] private List<VoxelData> _voxelData;
        private Dictionary<int, VoxelData> _voxelDataByID = new Dictionary<int, VoxelData>();

        private void Awake()
        {
            foreach (var voxelData in _voxelData)
                _voxelDataByID.Add(voxelData.id, voxelData);
        }    

        public VoxelData FindVoxelData(int id)
        {
            if (_voxelDataByID.ContainsKey(id))
                return _voxelDataByID[id];

            Debug.LogError($"VoxelData with id={id} does not exist");
            return null;
        }

        public List<VoxelData> GetAllData() => _voxelDataByID.Values.ToList();
    }
}