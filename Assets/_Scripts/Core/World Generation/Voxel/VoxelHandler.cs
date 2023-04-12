using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public static class VoxelHandler
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

        public static MeshData GenerateVoxel(ChunkData chunkData, MeshData data, Vector3Int position)
        {
            foreach (var direction in _directions)
            {
                Vector3Int neighbourVoxelCoordinates = position + direction.ToVector3Int();
                VoxelType neighbourVoxelType = ChunkVoxelData.GetVoxelDataAt(chunkData, neighbourVoxelCoordinates).type;
                
                if (neighbourVoxelType == VoxelType.Air)
                    RenderVoxelFace(data, position, direction);
            }

            return data;
        }

        private static MeshData RenderVoxelFace(MeshData data, Vector3Int position, Direction direction)
        {
            GenerateVoxelFace(data, position, direction);
            //TODO: Generate UVs

            return data;
        }

        private static void GenerateVoxelFace(MeshData meshData, Vector3 position, Direction direction)
        {
            switch (direction)
            {
                case Direction.up:
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z - VERTEX_OFFSET));
                    break;

                case Direction.down:
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z - VERTEX_OFFSET));
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
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y - VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x + VERTEX_OFFSET, position.y + VERTEX_OFFSET, position.z + VERTEX_OFFSET));
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
    }
}