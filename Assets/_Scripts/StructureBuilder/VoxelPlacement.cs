using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;

namespace HerosJourney.StructureBuilder
{
    public class VoxelPlacement
    {
        private StructureData _structureData;
        private VoxelStorage _voxelStorage;

        public VoxelPlacement(StructureData structureData, VoxelStorage storage)
        {
            _structureData = structureData;
            _voxelStorage = storage;
        }

        public void PlaceVoxel(int voxelId, Vector3Int position)
        {
            Voxel voxel = _voxelStorage.GetVoxelByID(voxelId);
            StructureDataHandler.SetVoxelAt(_structureData, voxel, position);
        }
    }
}