using HerosJourney.Core.WorldGeneration.Voxels;
using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Structures.Builder
{
    public class StructureBuilder : MonoBehaviour
    {
        [SerializeField] private VoxelData _voxelData;
        [SerializeField] private List<Vector3Int> _voxelPositions;

        private Dictionary<Vector3Int, VoxelData> _voxels = new Dictionary<Vector3Int, VoxelData>();

        private void Start()
        {
            foreach (var position in _voxelPositions)
                _voxels.Add(position, _voxelData);

            //CreateStructure();
            LoadStructure("tree");

            Debug.Log(_voxels);
        }

        public void CreateStructure()
        {
            StructureSaveLoad.SaveStructure(_voxels, "tree");
        }

        public void LoadStructure(string structureName)
        {
            _voxels = StructureSaveLoad.LoadStructure(structureName);
        }
    }
}