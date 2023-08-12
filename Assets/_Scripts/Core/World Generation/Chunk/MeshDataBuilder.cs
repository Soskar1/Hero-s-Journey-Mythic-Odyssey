using HerosJourney.Core.WorldGeneration.Voxels;
using HerosJourney.Utils;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public static class MeshDataBuilder
    {
        private static VoxelDataStorage _voxelDataStorage;

        private const float VERTEX_OFFSET = 0.5f;
        private static Direction[] _directions = {
            Direction.up,
            Direction.down,
            Direction.right,
            Direction.left,
            Direction.forward,
            Direction.back
        };

        public static void Initialize(VoxelDataStorage voxelDataStorage) => _voxelDataStorage = voxelDataStorage;

        public static MeshData GenerateMeshData(ChunkData chunkData)
        {
            MeshData meshData = new MeshData(true);

            for (int x = 0; x < chunkData.ChunkLength; ++x)
                for (int y = 0; y < chunkData.ChunkHeight; ++y)
                    for (int z = 0; z < chunkData.ChunkLength; ++z)
                        GenerateVoxelFaces(chunkData, meshData, _voxelDataStorage.Get(chunkData.voxelId[x, y, z]), new Vector3Int(x, y, z));

            return meshData;
        }

        private static void GenerateVoxelFaces(ChunkData chunkData, MeshData meshData, VoxelData voxelData, Vector3Int position)
        {
            if (voxelData.type == VoxelType.Air || voxelData.type == VoxelType.Nothing)
                return;

            foreach (var direction in _directions)
            {
                Vector3Int neighbourVoxelCoordinates = position + direction.ToVector3Int();
                int neighbourVoxelId = ChunkDataHandler.GetVoxelAt(chunkData, neighbourVoxelCoordinates);
                VoxelType neighbourVoxelType = _voxelDataStorage.Get(neighbourVoxelId).type;

                if (voxelData.type == VoxelType.Liquid && neighbourVoxelType != VoxelType.Air)
                    continue;
                
                if (neighbourVoxelType == VoxelType.Air || neighbourVoxelType == VoxelType.Liquid)
                    SetVoxelFace(meshData, voxelData, position, direction);
            }
        }

        private static void SetVoxelFace(MeshData meshData, VoxelData voxelData, Vector3Int position, Direction direction)
        {
            CreateQuad(meshData, position, direction, voxelData.generatesCollider);
            AssignUVCoordinates(meshData, voxelData, direction);
        }

        private static void CreateQuad(MeshData meshData, Vector3 position, Direction direction, bool generatesCollider)
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

            meshData.AddVertices(vertices, generatesCollider);
            meshData.CreateQuad(vertices, generatesCollider);
        }

        private static void AssignUVCoordinates(MeshData meshData, VoxelData voxelData, Direction direction)
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

            meshData.AddUVCoordinates(uvs);
        }
    }
}