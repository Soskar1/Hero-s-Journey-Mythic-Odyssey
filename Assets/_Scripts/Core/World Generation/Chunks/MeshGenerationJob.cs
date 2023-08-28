using HerosJourney.Utils;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    [BurstCompile(CompileSynchronously = true)]
    public struct MeshGenerationJob : IJob
    {
        public struct NeighbourChunks
        {
            public TSChunkData forwardChunk;
            public TSChunkData rightChunk;
            public TSChunkData backChunk;
            public TSChunkData leftChunk;

            public TSChunkData GetNeighbourChunkAtPosition(int3 position)
            {
                if (math.all(position == forwardChunk.WorldPosition))
                    return forwardChunk;

                if (math.all(position == rightChunk.WorldPosition))
                    return rightChunk;

                if (math.all(position == backChunk.WorldPosition))
                    return backChunk;

                return leftChunk;
            }
        }

        public struct VoxelGeometry
        {
            public NativeArray<int3> vertices;
            public NativeArray<int> triangles;
        }

        [WriteOnly] public MeshData meshData;
        [ReadOnly] public TSChunkData chunkData;
        [ReadOnly] public VoxelGeometry voxelGeometry;
        [ReadOnly] public NeighbourChunks neighbourChunks;

        private int vCount;
        private const int _FACES_ = 6;
        private const int _SCALE_ = 1;
        private const int _VERTEX_COUNT_ = 4;

        public void Execute()
        {
            for (int x = 0; x < chunkData.Length; ++x)
            {
                for (int z = 0; z < chunkData.Length; ++z)
                {
                    for (int y = 0; y < chunkData.Height; ++y)
                    {
                        if (chunkData.voxels[VoxelExtensions.GetVoxelIndex(new int3(x, y, z))].IsEmpty())
                            continue;

                        for (int i = 0; i < _FACES_; ++i)
                        {
                            var direction = (Direction) i;
                            int3 localPosition = new int3(x, y, z);
                            int3 neighbourPosition = localPosition + direction.ToInt3();
                            VoxelType neighbourVoxelType = GetVoxelAt(neighbourPosition);

                            if (neighbourVoxelType == VoxelType.Transparent)
                                CreateFace(direction, localPosition);
                        }
                    }
                }
            }
        }

        private VoxelType GetVoxelAt(int3 localPosition)
        {
            if (ChunkExtensions.IsInBounds(chunkData, localPosition))
            {
                int index = VoxelExtensions.GetVoxelIndex(localPosition);
                return chunkData.voxels[index];
            }

            return GetVoxelAtNeighbourChunks(chunkData.WorldPosition + localPosition);
        }

        private VoxelType GetVoxelAtNeighbourChunks(int3 worldPosition)
        {
            int3 chunkPosition = GetChunkPosition(worldPosition);
            TSChunkData neighbourChunk = neighbourChunks.GetNeighbourChunkAtPosition(chunkPosition);
            
            int3 localPosition = WorldToLocalPosition(neighbourChunk, worldPosition);
            int index = VoxelExtensions.GetVoxelIndex(localPosition);
            return neighbourChunk.voxels[index];
        }

        private int3 GetChunkPosition(int3 worldPosition)
        {
            return new int3
            {
                x = Mathf.FloorToInt(worldPosition.x / (float)chunkData.Length) * chunkData.Length,
                y = Mathf.FloorToInt(worldPosition.y / (float)chunkData.Height) * chunkData.Height,
                z = Mathf.FloorToInt(worldPosition.z / (float)chunkData.Length) * chunkData.Length
            };
        }

        private int3 WorldToLocalPosition(TSChunkData chunkData, int3 worldPosition)
        {
            return new int3
            {
                x = worldPosition.x - chunkData.WorldPosition.x,
                y = worldPosition.y - chunkData.WorldPosition.y,
                z = worldPosition.z - chunkData.WorldPosition.z
            };
        }

        private void CreateFace(Direction direction, int3 localPosition)
        {
            var vertices = GetFaceVertices(direction, localPosition);
            meshData.vertices.AddRange(vertices);
            vertices.Dispose();

            AddTriangles();
        }

        private NativeArray<int3> GetFaceVertices(Direction direction, int3 localPosition)
        {
            var faceVertices = new NativeArray<int3>(_VERTEX_COUNT_, Allocator.Temp);

            for (int i = 0; i < faceVertices.Length; ++i)
            {
                var index = voxelGeometry.triangles[(int)direction * _VERTEX_COUNT_ + i];
                faceVertices[i] = voxelGeometry.vertices[index] * _SCALE_ + localPosition;
            }

            return faceVertices;
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
    }
}