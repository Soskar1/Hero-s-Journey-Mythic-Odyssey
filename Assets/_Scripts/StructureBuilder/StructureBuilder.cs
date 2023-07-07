using UnityEngine;
using Zenject;

namespace HerosJourney.StructureBuilder
{
    public class StructureBuilder : IInitializable
    {
        private StructureData _structureData;
        private StructureMesh _structureMesh;
        private StructureRenderer _structureRenderer;
        private VoxelPlacement _voxelPlacement;
        private int _groundID;

        public StructureBuilder(StructureData structureData, StructureRenderer renderer, VoxelPlacement voxelPlacement, int groundID)
        {
            _structureData = structureData;
            _structureRenderer = renderer;
            _voxelPlacement = voxelPlacement;

            _groundID = groundID;
        }

        public void Initialize()
        {
            PlaceGround();

            _structureData.mesh = StructureMeshBuilder.GenerateMeshData(_structureData);

            _structureRenderer.UpdateStructure();
        }

        private void PlaceGround()
        {
            for (int x = 0; x < _structureData.Size.x; ++x)
                for (int z = 0; z < _structureData.Size.z; ++z)
                    _voxelPlacement.PlaceVoxel(_groundID, new Vector3Int(x, 0, z));
        }
    }
}