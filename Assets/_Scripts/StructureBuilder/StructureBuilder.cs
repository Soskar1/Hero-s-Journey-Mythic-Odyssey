using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.StructureBuilder
{
    public class StructureBuilder : MonoBehaviour
    {
        private List<VoxelSaveData> _savedVoxels;

        public void SaveStructure(string structureName) => StructureSaveLoad.SaveStructure(_savedVoxels, structureName);

        public void LoadStructure(string structureName) => _savedVoxels = StructureSaveLoad.LoadStructure(structureName);
    }
}