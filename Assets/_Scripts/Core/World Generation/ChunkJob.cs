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

        public struct BlockData
        {
            public NativeArray<int3> vertices;
            public NativeArray<int> triangles;
        }

        public struct ChunkData
        {
            public NativeArray<Block> blocks;
        }

        [WriteOnly] public MeshData meshData;
        [ReadOnly] public ChunkData chunkData;
        [ReadOnly] public BlockData blockData;

        private int vCount;

        public void Execute()
        {
            for (int x = 0; x < 16; ++x)
            {
                for (int z = 0; z < 16; ++z)
                {
                    for (int y = 0; y < 128; ++y)
                    {
                        if (chunkData.blocks[BlockExtensions.GetBlockIndex(new int3(x, y, z))].IsEmpty())
                            continue;

                        for (int i = 0; i < 6; ++i)
                        {
                            var direction = (Direction) i;
                            int3 position = new int3(x, y, z);
                            int3 neigbourPosition = position + direction.ToInt3();
                            if (Check(neigbourPosition))
                                CreateFace(direction, position);
                        }
                    }
                }
            }
        }

        private void CreateFace(Direction direction, int3 position)
        {
            var vertices = GetFaceVertices(direction, 1, position);
            
            meshData.vertices.AddRange(vertices);

            vertices.Dispose();

            vCount += 4;

            meshData.triangles.Add(vCount - 4);
            meshData.triangles.Add(vCount - 4 + 1);
            meshData.triangles.Add(vCount - 4 + 2);
            meshData.triangles.Add(vCount - 4);
            meshData.triangles.Add(vCount - 4 + 2);
            meshData.triangles.Add(vCount - 4 + 3);
        }

        private bool Check(int3 position)
        {
            if (position.x >= 16 || position.z >= 16
             || position.x < 0 || position.z < 0 || position.y < 0)
             return true;

            if (position.y >= 128)
                return false;

            return chunkData.blocks[BlockExtensions.GetBlockIndex(position)].IsEmpty();
        }

        private NativeArray<int3> GetFaceVertices(Direction direction, int scale, int3 position)
        {
            var faceVertices = new NativeArray<int3>(4, Allocator.Temp);

            for (int i = 0; i < faceVertices.Length; ++i)
            {
                var index = blockData.triangles[(int)direction * 4 + i];
                faceVertices[i] = blockData.vertices[index] * scale + position;
            }

            return faceVertices;
        }
    }
}