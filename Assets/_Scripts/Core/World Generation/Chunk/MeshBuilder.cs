using HerosJourney.Core.WorldGeneration.Voxels;
using HerosJourney.Utils;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public static class MeshBuilder
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

        public static MeshData GenerateMeshData(ChunkData chunkData)
        {
            MeshData meshData = new MeshData(true);

            for (int x = 0; x < chunkData.ChunkLength; ++x)
                for (int y = 0; y < chunkData.ChunkHeight; ++y)
                    for (int z = 0; z < chunkData.ChunkLength; ++z)
                        GenerateVoxelFaces(chunkData, meshData, chunkData.voxels[x, y, z].data, new Vector3Int(x, y, z));

            return meshData;
        }

        private static void GenerateVoxelFaces(ChunkData chunkData, MeshData meshData, VoxelData voxelData, Vector3Int position)
        {
            if (voxelData.type == VoxelType.Air || voxelData.type == VoxelType.Nothing)
                return;

            foreach (var direction in _directions)
            {
                Vector3Int neighbourVoxelCoordinates = position + direction.ToVector3Int();
                Voxel neighbourVoxel = ChunkDataHandler.GetVoxelAt(chunkData, neighbourVoxelCoordinates);
                VoxelType neighbourVoxelType = VoxelType.Nothing;

                if (neighbourVoxel != null)
                    neighbourVoxelType = neighbourVoxel.GetType();

                if (voxelData.type == VoxelType.Liquid && neighbourVoxelType != VoxelType.Air)
                    continue;
                
                if (neighbourVoxelType == VoxelType.Air || neighbourVoxelType == VoxelType.Liquid)
                    SetVoxelFace(meshData, voxelData, position, direction);
            }
        }

        private static void SetVoxelFace(MeshData meshData, VoxelData voxelData, Vector3Int position, Direction direction)
        {
            AddVerticies(meshData, position, direction, voxelData.generatesCollider);
            AssignUVCoordinates(meshData, voxelData, direction);
        }

        private static void AddVerticies(MeshData meshData, Vector3 position, Direction direction, bool generatesCollider)
        {
            switch (direction)
            {
                case Direction.up:
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET), generatesCollider);
                    break;

                case Direction.down:
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET), generatesCollider);
                    break;

                case Direction.right:
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET), generatesCollider);
                    break;

                case Direction.left:
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET), generatesCollider);
                    break;

                case Direction.forward:
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET), generatesCollider);
                    break;

                case Direction.back:
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET), generatesCollider);
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET), generatesCollider);
                    break;
            }

            meshData.CreateQuad(generatesCollider);

            for (int i = 0; i < 4; ++i)
                meshData.AddNormal(direction.ToVector3Int());
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

            meshData.AddUVCoordinates(uvs[0]);
            meshData.AddUVCoordinates(uvs[1]);
            meshData.AddUVCoordinates(uvs[2]);
            meshData.AddUVCoordinates(uvs[3]);
        }
    }
}