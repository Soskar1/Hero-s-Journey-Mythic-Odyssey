using System.Collections.Generic;
using HerosJourney.Core.WorldGeneration.Voxels;
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
            foreach (Vector3Int position in _voxelPositions)
                _voxels.Add(position, _voxelData);

            //CreateStructure();
        }

        public void CreateStructure()
        {
            StructureSaveLoad.SaveStructure(_voxels, "tree");
        }
    }
}