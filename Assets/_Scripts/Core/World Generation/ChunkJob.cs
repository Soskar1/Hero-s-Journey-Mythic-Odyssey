using HerosJourney.Utils;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration
{
    [BurstCompile(CompileSynchronously = true)]
    public struct ChunkJob : IJob
    {
        public struct MeshData
        {
            public NativeList<int3> vertices;
            public NativeList<int> triangles;
        }

        public struct VoxelGeometry
        {
            public NativeArray<int3> vertices;
            public NativeArray<int> triangles;
        }

        public struct ChunkData
        {
            public NativeArray<VoxelType> voxels;
            public byte length;
            public byte height;
        }

        [WriteOnly] public MeshData meshData;
        [ReadOnly] public ChunkData chunkData;
        [ReadOnly] public VoxelGeometry voxelGeometry;

        private int vCount;

        public void Execute()
        {
            for (int x = 0; x < chunkData.length; ++x)
            {
                for (int z = 0; z < chunkData.length; ++z)
                {
                    for (int y = 0; y < chunkData.height; ++y)
                    {
                        if (chunkData.voxels[VoxelExtensions.GetVoxelIndex(new int3(x, y, z))].IsEmpty())
                            continue;

                        for (int i = 0; i < 6; ++i)
                        {
                            var direction = (Direction) i;
                            int3 localPosition = new int3(x, y, z);
                            int3 neigbourPosition = localPosition + direction.ToInt3();
                            
                            if (!IsInBounds(neigbourPosition) || chunkData.voxels[VoxelExtensions.GetVoxelIndex(neigbourPosition)].IsEmpty())
                                CreateFace(direction, localPosition);
                        }
                    }
                }
            }
        }

        private void CreateFace(Direction direction, int3 localPosition)
        {
            var vertices = GetFaceVertices(direction, 1, localPosition);
            meshData.vertices.AddRange(vertices);
            vertices.Dispose();

            AddTriangles();
        }

        private void AddTriangles()
        {
            vCount += 4;

            meshData.triangles.Add(vCount - 4);
            meshData.triangles.Add(vCount - 4 + 1);
            meshData.triangles.Add(vCount - 4 + 2);
            meshData.triangles.Add(vCount - 4);
            meshData.triangles.Add(vCount - 4 + 2);
            meshData.triangles.Add(vCount - 4 + 3);
        }

        private bool IsInBounds(int3 localPosition)
        {
            if (localPosition.x < 0 || localPosition.x >= chunkData.length ||
                localPosition.y < 0 || localPosition.y >= chunkData.height ||
                localPosition.z < 0 || localPosition.z >= chunkData.length)
                return false;

            return true;
        }

        private NativeArray<int3> GetFaceVertices(Direction direction, int scale, int3 localPosition)
        {
            var faceVertices = new NativeArray<int3>(4, Allocator.Temp);

            for (int i = 0; i < faceVertices.Length; ++i)
            {
                var index = voxelGeometry.triangles[(int)direction * 4 + i];
                faceVertices[i] = voxelGeometry.vertices[index] * scale + localPosition;
            }

            return faceVertices;
        }
    }
}