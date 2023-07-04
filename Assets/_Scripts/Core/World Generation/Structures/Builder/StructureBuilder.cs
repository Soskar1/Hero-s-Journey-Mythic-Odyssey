using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Structures.Builder
{
    public class StructureBuilder : MonoBehaviour
    {
        [SerializeField] private List<VoxelSaveData> _savedVoxels;

        public void CreateStructure(string structureName) => StructureSaveLoad.SaveStructure(_savedVoxels, structureName);

        public void LoadStructure(string structureName)
        {
            _savedVoxels = StructureSaveLoad.LoadStructure(structureName);
        }
    }
}