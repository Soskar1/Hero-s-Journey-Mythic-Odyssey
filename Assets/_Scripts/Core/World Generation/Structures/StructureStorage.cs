using HerosJourney.StructureBuilder.Saving;
using HerosJourney.Utils;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration.Structures
{
    public class StructureStorage : Storage<string, List<VoxelSaveData>>, IInitializable
    {
        private List<TextAsset> _structureFiles = new List<TextAsset>();

        public StructureStorage(List<TextAsset> structureFiles) => _structureFiles = structureFiles;

        public void Initialize()
        {
            foreach (var file in _structureFiles)
            {
                var structure = StructureSaveLoad.LoadStructure(file);
                Add(file.name, structure);
            }
        }
    }
}