using HerosJourney.Core.WorldGeneration.Voxels;
using HerosJourney.Utils;
using UnityEngine;

namespace HerosJourney.StructureBuilder
{
    public static class StructureMeshBuilder
    {
        private const float VERTEX_OFFSET = 0.5f;
        private static Direction[] _directions = {
            Direction.up,
            Direction.down,
            Direction.right,
            Direction.left,
            Direction.forward,
            Direction.back
        };

        public static StructureMesh GenerateMeshData(StructureData structureData)
        {
            StructureMesh structureMesh = new StructureMesh();

            for (int x = 0; x < structureData.Size.x; ++x)
                for (int y = 0; y < structureData.Size.y; ++y)
                    for (int z = 0; z < structureData.Size.z; ++z)
                        if (structureData.voxels[x, y, z] != null)
                            GenerateVoxelFaces(structureData, structureMesh, structureData.voxels[x, y, z].data, new Vector3Int(x, y, z));

            return structureMesh;
        }

        private static void GenerateVoxelFaces(StructureData structureData, StructureMesh structureMesh, VoxelData voxelData, Vector3Int position)
        {
            if (voxelData.type is VoxelType.Air || voxelData.type is VoxelType.Nothing)
                return;

            foreach (var direction in _directions)
            {
                Vector3Int neighbourVoxelCoordinates = position + direction.ToVector3Int();
                Voxel neighbourVoxel = StructureDataHandler.GetVoxelAt(structureData, neighbourVoxelCoordinates);
                VoxelType neighbourVoxelType = neighbourVoxel is null ? VoxelType.Nothing : neighbourVoxel.VoxelType;

                if (neighbourVoxelType != VoxelType.Solid)
                    SetVoxelFace(structureMesh, voxelData, position, direction);
            }
        }

        private static void SetVoxelFace(StructureMesh structureMesh, VoxelData voxelData, Vector3Int position, Direction direction)
        {
            CreateQuad(structureMesh, position, direction);
            AssignUVCoordinates(structureMesh, voxelData, direction);
        }

        private static void CreateQuad(StructureMesh structureMesh, Vector3 position, Direction direction)
        {
            Vector3[] vertices = new Vector3[4];

            switch (direction)
            {
                case Direction.up:
                    vertices[0] = new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET);
                    vertices[1] = new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET);
                    vertices[2] = new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET);
                    vertices[3] = new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET);
                    break;

                case Direction.down:
                    vertices[0] = new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET);
                    vertices[1] = new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET);
                    vertices[2] = new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET);
                    vertices[3] = new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET);
                    break;

                case Direction.right:
                    vertices[0] = new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET);
                    vertices[1] = new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET);
                    vertices[2] = new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET);
                    vertices[3] = new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET);
                    break;

                case Direction.left:
                    vertices[0] = new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET);
                    vertices[1] = new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET);
                    vertices[2] = new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET);
                    vertices[3] = new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET);
                    break;

                case Direction.forward:
                    vertices[0] = new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET);
                    vertices[1] = new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET);
                    vertices[2] = new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET);
                    vertices[3] = new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET);
                    break;

                case Direction.back:
                    vertices[0] = new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET);
                    vertices[1] = new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET);
                    vertices[2] = new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET);
                    vertices[3] = new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET);
                    break;
            }

            structureMesh.AddVertices(vertices);
            structureMesh.CreateQuad(vertices);
        }

        private static void AssignUVCoordinates(StructureMesh structureMesh, VoxelData voxelData, Direction direction)
        {
            if (voxelData.type == VoxelType.Air)
                return;

            Vector2[] uvs = new Vector2[4];

            switch (direction)
            {
                case Direction.up:
                    uvs = UVMapping.GetUVCoordinates(voxelData.textureData.up);
                    break;

                case Direction.down:
                    uvs = UVMapping.GetUVCoordinates(voxelData.textureData.down);
                    break;

                default:
                    uvs = UVMapping.GetUVCoordinates(voxelData.textureData.side);
                    break;
            }

            structureMesh.AddUVCoordinates(uvs);
        }
    }
}