using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Structures.Builder
{
    public class StructureBuilder : MonoBehaviour
    {
        [SerializeField] private List<VoxelSaveData> _savedVoxels;

        private void Start()
        {
            //CreateStructure();
            LoadStructure("tree");
        }

        public void CreateStructure()
        {
            StructureSaveLoad.SaveStructure(_savedVoxels, "tree");
        }

        public void LoadStructure(string structureName)
        {
            _savedVoxels = StructureSaveLoad.LoadStructure(structureName);
        }
    }
}