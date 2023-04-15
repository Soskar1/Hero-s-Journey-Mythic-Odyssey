using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    public static class VoxelFaceGeneration
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

        public static MeshData GenerateVoxel(ChunkData chunkData, MeshData meshData, Vector3Int position, VoxelType voxelType)
        {
            if (voxelType == VoxelType.Air || voxelType == VoxelType.Nothing)
                return meshData;

            foreach (var direction in _directions)
            {
                Vector3Int neighbourVoxelCoordinates = position + direction.ToVector3Int();
                Voxel neighbourVoxel = ChunkVoxelData.GetVoxelAt(chunkData, neighbourVoxelCoordinates);
                VoxelType neighbourVoxelType = VoxelType.Nothing;

                if (neighbourVoxel != null)
                    neighbourVoxelType = neighbourVoxel.GetType();
                
                if (neighbourVoxelType == VoxelType.Air)
                    RenderVoxelFace(meshData, position, direction);
            }

            return meshData;
        }

        private static MeshData RenderVoxelFace(MeshData data, Vector3Int position, Direction direction)
        {
            GenerateVoxelFace(data, position, direction);
            GenerateUVs();

            return data;
        }

        private static void GenerateVoxelFace(MeshData meshData, Vector3 position, Direction direction)
        {
            switch (direction)
            {
                case Direction.up:
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET));
                    break;

                case Direction.down:
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    break;

                case Direction.right:
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    break;

                case Direction.left:
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET));
                    break;

                case Direction.forward:
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    break;

                case Direction.back:
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET));
                    break;
            }

            meshData.CreateQuad();
        }

        private static void GenerateUVs()
        {

        }
    }
}