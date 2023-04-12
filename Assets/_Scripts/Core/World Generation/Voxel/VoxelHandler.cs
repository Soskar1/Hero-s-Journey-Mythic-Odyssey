using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public static class VoxelHandler
    {
        private const float VERTEX_OFFSET = 0.5f;

        public static void GenerateVoxelFace(MeshData meshData, Vector3 position, Direction direction)
        {
            switch (direction)
            {
                case Direction.up:
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y, position.z + VERTEX_OFFSET));
                    meshData.AddVertex(new Vector3(position.x - VERTEX_OFFSET, position.y, position.z + VERTEX_OFFSET));
                    break;

                case Direction.down:
                    break;

                case Direction.right:
                    break;

                case Direction.left:
                    break;

                case Direction.forward:
                    break;

                case Direction.back:
                    break;
            }
        }
    }
}