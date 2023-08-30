using HerosJourney.Core.WorldGeneration.Voxels;
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
            public ThreadSafeChunkData forwardChunk;
            public ThreadSafeChunkData rightChunk;
            public ThreadSafeChunkData backChunk;
            public ThreadSafeChunkData leftChunk;

            public ThreadSafeChunkData GetNeighbourChunkAtPosition(int3 position)
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

        public struct TextureData
        {
            public int tileSize;
            public float xStep;
            public float yStep;
        }

        [WriteOnly] public ThreadSafeMeshData meshData;
        [ReadOnly] public ThreadSafeChunkData chunkData;
        [ReadOnly] public NeighbourChunks neighbourChunks;
        [ReadOnly] public TextureData textureData;
        [ReadOnly] public NativeHashMap<int, ThreadSafeVoxelData> storage;

        private int vCount;
        private const int _FACES_ = 6;
        private const int _VERTEX_COUNT_ = 4;
        private const float _VERTEX_OFFSET_ = 0.5f;
        private const float _TEXTURE_OFFSET_ = 0.001f;

        public void Execute()
        {
            for (int x = 0; x < chunkData.Length; ++x)
            {
                for (int z = 0; z < chunkData.Length; ++z)
                {
                    for (int y = 0; y < chunkData.Height; ++y)
                    {
                        int3 localPosition = new int3(x, y, z);
                        var voxelData = GetVoxelData(chunkData, localPosition);

                        if (voxelData.type.IsEmpty())
                            continue;

                        for (int i = 0; i < _FACES_; ++i)
                        {
                            var direction = (Direction) i;
                            
                            int3 neighbourPosition = localPosition + direction.ToInt3();
                            var neighbourVoxelData = GetVoxelAt(neighbourPosition);

                            if (neighbourVoxelData.type.IsEmpty())
                                CreateFace(voxelData, direction, localPosition);
                        }
                    }
                }
            }
        }

        private void CreateFace(ThreadSafeVoxelData voxelData, Direction direction, int3 localPosition)
        {
            var vertices = GetFaceVertices(direction, localPosition);
            meshData.vertices.AddRange(vertices);
            vertices.Dispose();

            AddTriangles();
            AssignUVs(voxelData, direction);
        }

        private ThreadSafeVoxelData GetVoxelData(ThreadSafeChunkData chunkData, int3 localPosition)
        {
            int index = VoxelExtensions.GetVoxelIndex(localPosition);
            ushort voxelID = chunkData.voxels[index];
            return storage[voxelID];
        }

        private ThreadSafeVoxelData GetVoxelAt(int3 localPosition)
        {
            if (ChunkExtensions.IsInBounds(chunkData, localPosition))
                return GetVoxelData(chunkData, localPosition);

            return GetVoxelAtNeighbourChunks(chunkData.WorldPosition + localPosition);
        }

        private ThreadSafeVoxelData GetVoxelAtNeighbourChunks(int3 worldPosition)
        {
            int3 chunkPosition = GetChunkPosition(worldPosition);
            ThreadSafeChunkData neighbourChunk = neighbourChunks.GetNeighbourChunkAtPosition(chunkPosition);
            
            int3 localPosition = WorldToLocalPosition(neighbourChunk, worldPosition);
            return GetVoxelData(neighbourChunk, localPosition);
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

        private int3 WorldToLocalPosition(ThreadSafeChunkData chunkData, int3 worldPosition)
        {
            return new int3
            {
                x = worldPosition.x - chunkData.WorldPosition.x,
                y = worldPosition.y - chunkData.WorldPosition.y,
                z = worldPosition.z - chunkData.WorldPosition.z
            };
        }

        private NativeArray<float3> GetFaceVertices(Direction direction, int3 localPosition)
        {
            var faceVertices = new NativeArray<float3>(_VERTEX_COUNT_, Allocator.Temp);

            switch (direction)
            {
                case Direction.up:
                    faceVertices[0] = new float3(localPosition.x - _VERTEX_OFFSET_, localPosition.y + _VERTEX_OFFSET_, localPosition.z + _VERTEX_OFFSET_);
                    faceVertices[1] = new float3(localPosition.x + _VERTEX_OFFSET_, localPosition.y + _VERTEX_OFFSET_, localPosition.z + _VERTEX_OFFSET_);
                    faceVertices[2] = new float3(localPosition.x + _VERTEX_OFFSET_, localPosition.y + _VERTEX_OFFSET_, localPosition.z - _VERTEX_OFFSET_);
                    faceVertices[3] = new float3(localPosition.x - _VERTEX_OFFSET_, localPosition.y + _VERTEX_OFFSET_, localPosition.z - _VERTEX_OFFSET_);
                    break;

                case Direction.down:
                    faceVertices[0] = new float3(localPosition.x - _VERTEX_OFFSET_, localPosition.y - _VERTEX_OFFSET_, localPosition.z - _VERTEX_OFFSET_);
                    faceVertices[1] = new float3(localPosition.x + _VERTEX_OFFSET_, localPosition.y - _VERTEX_OFFSET_, localPosition.z - _VERTEX_OFFSET_);
                    faceVertices[2] = new float3(localPosition.x + _VERTEX_OFFSET_, localPosition.y - _VERTEX_OFFSET_, localPosition.z + _VERTEX_OFFSET_);
                    faceVertices[3] = new float3(localPosition.x - _VERTEX_OFFSET_, localPosition.y - _VERTEX_OFFSET_, localPosition.z + _VERTEX_OFFSET_);
                    break;

                case Direction.right:
                    faceVertices[0] = new float3(localPosition.x + _VERTEX_OFFSET_, localPosition.y - _VERTEX_OFFSET_, localPosition.z - _VERTEX_OFFSET_);
                    faceVertices[1] = new float3(localPosition.x + _VERTEX_OFFSET_, localPosition.y + _VERTEX_OFFSET_, localPosition.z - _VERTEX_OFFSET_);
                    faceVertices[2] = new float3(localPosition.x + _VERTEX_OFFSET_, localPosition.y + _VERTEX_OFFSET_, localPosition.z + _VERTEX_OFFSET_);
                    faceVertices[3] = new float3(localPosition.x + _VERTEX_OFFSET_, localPosition.y - _VERTEX_OFFSET_, localPosition.z + _VERTEX_OFFSET_);
                    break;

                case Direction.left:
                    faceVertices[0] = new float3(localPosition.x - _VERTEX_OFFSET_, localPosition.y - _VERTEX_OFFSET_, localPosition.z + _VERTEX_OFFSET_);
                    faceVertices[1] = new float3(localPosition.x - _VERTEX_OFFSET_, localPosition.y + _VERTEX_OFFSET_, localPosition.z + _VERTEX_OFFSET_);
                    faceVertices[2] = new float3(localPosition.x - _VERTEX_OFFSET_, localPosition.y + _VERTEX_OFFSET_, localPosition.z - _VERTEX_OFFSET_);
                    faceVertices[3] = new float3(localPosition.x - _VERTEX_OFFSET_, localPosition.y - _VERTEX_OFFSET_, localPosition.z - _VERTEX_OFFSET_);
                    break;

                case Direction.forward:
                    faceVertices[0] = new float3(localPosition.x + _VERTEX_OFFSET_, localPosition.y - _VERTEX_OFFSET_, localPosition.z + _VERTEX_OFFSET_);
                    faceVertices[1] = new float3(localPosition.x + _VERTEX_OFFSET_, localPosition.y + _VERTEX_OFFSET_, localPosition.z + _VERTEX_OFFSET_);
                    faceVertices[2] = new float3(localPosition.x - _VERTEX_OFFSET_, localPosition.y + _VERTEX_OFFSET_, localPosition.z + _VERTEX_OFFSET_);
                    faceVertices[3] = new float3(localPosition.x - _VERTEX_OFFSET_, localPosition.y - _VERTEX_OFFSET_, localPosition.z + _VERTEX_OFFSET_);
                    break;

                case Direction.back:
                    faceVertices[0] = new float3(localPosition.x - _VERTEX_OFFSET_, localPosition.y - _VERTEX_OFFSET_, localPosition.z - _VERTEX_OFFSET_);
                    faceVertices[1] = new float3(localPosition.x - _VERTEX_OFFSET_, localPosition.y + _VERTEX_OFFSET_, localPosition.z - _VERTEX_OFFSET_);
                    faceVertices[2] = new float3(localPosition.x + _VERTEX_OFFSET_, localPosition.y + _VERTEX_OFFSET_, localPosition.z - _VERTEX_OFFSET_);
                    faceVertices[3] = new float3(localPosition.x + _VERTEX_OFFSET_, localPosition.y - _VERTEX_OFFSET_, localPosition.z - _VERTEX_OFFSET_);
                    break;
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

        private void AssignUVs(ThreadSafeVoxelData voxelData, Direction direction)
        {
            if (voxelData.type.IsEmpty())
                return;

            switch (direction)
            {
                case Direction.up:
                    AddUVs(voxelData.textureData.up);
                    break;

                case Direction.down:
                    AddUVs(voxelData.textureData.down);
                    break;

                default:
                    AddUVs(voxelData.textureData.side);
                    break;
            }
        }

        private void AddUVs(int2 textureCoordinates)
        {
            meshData.uvs.Add(new float2(textureData.xStep * textureCoordinates.x + _TEXTURE_OFFSET_, textureData.yStep * textureCoordinates.y + _TEXTURE_OFFSET_));
            meshData.uvs.Add(new float2(textureData.xStep * textureCoordinates.x + _TEXTURE_OFFSET_, textureData.yStep * textureCoordinates.y + textureData.yStep - _TEXTURE_OFFSET_));
            meshData.uvs.Add(new float2(textureData.xStep * textureCoordinates.x + textureData.xStep - _TEXTURE_OFFSET_, textureData.yStep * textureCoordinates.y + textureData.yStep - _TEXTURE_OFFSET_));
            meshData.uvs.Add(new float2(textureData.xStep * textureCoordinates.x + textureData.xStep - _TEXTURE_OFFSET_, textureData.yStep * textureCoordinates.y + _TEXTURE_OFFSET_));
        }
    }
}