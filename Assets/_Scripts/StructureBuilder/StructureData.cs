using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;

namespace HerosJourney.StructureBuilder
{
    public class StructureData
    {
        public Voxel[,,] voxels;
        public StructureMesh mesh;
        public Vector3Int Size { get; private set; }

        public StructureData(Vector3Int size)
        {
            voxels = new Voxel[size.x, size.y, size.z];
            Size = size;
        }
    }
}