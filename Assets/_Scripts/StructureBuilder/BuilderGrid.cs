using HerosJourney.StructureBuilder.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.StructureBuilder
{
    public class BuilderGrid : MonoBehaviour
    {
        [SerializeField] private Vector3Int _size;
        private List<VoxelSaveData> _voxels;
    }
}