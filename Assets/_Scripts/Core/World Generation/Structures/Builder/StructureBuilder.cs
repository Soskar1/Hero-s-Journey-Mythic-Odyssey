using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Structures.Builder
{
    public class StructureBuilder : MonoBehaviour
    {
        [SerializeField] private List<VoxelSaveData> _savedVoxels;

        [ContextMenu("Save Structure")]
        public void SaveStructure() => StructureSaveLoad.SaveStructure(_savedVoxels, "tree");

        public void LoadStructure(string structureName)
        {
            _savedVoxels = StructureSaveLoad.LoadStructure(structureName);
        }
    }
}