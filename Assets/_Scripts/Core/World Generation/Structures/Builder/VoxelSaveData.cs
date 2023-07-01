using HerosJourney.Core.WorldGeneration.Voxels;
using System;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Structures.Builder
{
    [Serializable]
    public struct VoxelSaveData
    {
        public Vector3Int position;
        public VoxelData voxelData;
    }
}
