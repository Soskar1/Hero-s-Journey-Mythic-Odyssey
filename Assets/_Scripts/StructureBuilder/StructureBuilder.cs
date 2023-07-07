using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;
using Zenject;

namespace HerosJourney.StructureBuilder
{
    public class StructureBuilder : IInitializable
    {
        private StructureData _structureData;
        
        private VoxelStorage _voxelStorage;
        private Voxel _groundVoxel;
        private int _groundID;

        public StructureBuilder(StructureData structureData, VoxelStorage voxelStorage, int groundID)
        {
            _voxelStorage = voxelStorage;
            _structureData = structureData;

            _groundID = groundID;
        }

        public void Initialize()
        {
            _groundVoxel = _voxelStorage.GetVoxelByID(_groundID);

            for (int x = 0; x < _structureData.Size.x; ++x)
                for (int z = 0; z < _structureData.Size.z; ++z)
                    StructureDataHandler.SetVoxelAt(_structureData, _groundVoxel, new Vector3Int(x, 0, z));

            _structureData.mesh = StructureMeshBuilder.GenerateMeshData(_structureData);
        }
    }
}