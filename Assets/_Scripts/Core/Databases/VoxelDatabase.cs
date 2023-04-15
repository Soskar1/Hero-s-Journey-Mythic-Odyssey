using HerosJourney.Core.WorldGeneration.Voxels;
using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.Databases
{
    public class VoxelDatabase : Database<VoxelData>
    {
        [SerializeField] private List<VoxelData> data;

        private void Awake()
        {
            foreach (VoxelData voxel in data)
                if (voxel != null)
                    Add(voxel, voxel);
        }
    }
}