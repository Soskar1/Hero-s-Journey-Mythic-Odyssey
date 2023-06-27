using UnityEngine;
using System.Collections.Generic;
using HerosJourney.Core.WorldGeneration.Voxels;

namespace HerosJourney.Core.WorldGeneration.Structures
{
    public class StructureData
    {
        public List<Vector2Int> structurePositions = new List<Vector2Int>();
        public List<Voxel> voxels = new List<Voxel>();
    }
}