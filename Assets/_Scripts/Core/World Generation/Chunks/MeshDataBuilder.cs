using HerosJourney.Core.WorldGeneration.Voxels;
using HerosJourney.Utils;
using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class MeshDataBuilder : IInitializable, IDisposable
    {
        private const float _VERTEX_OFFSET = 0.5f;
        private readonly VoxelDataTSStorage _voxelDataStorage;
        private readonly Texture2D _textureAtlas;
        private readonly int _tileSize;
        private readonly float _xStep;
        private readonly float _yStep;
        private NativeArray<Direction> _directions;
        private NativeArray<float3> _vertexOffsets;

        public MeshDataBuilder(VoxelDataTSStorage voxelDataStorage, Texture2D textureAtlas, int tileSize)
        {
            _voxelDataStorage = voxelDataStorage;
            _textureAtlas = textureAtlas;
            _tileSize = tileSize;

            _xStep = _tileSize / (float)_textureAtlas.width;
            _yStep = _tileSize / (float)_textureAtlas.height;
        }

        public void Initialize()
        {
            _directions = new NativeArray<Direction>(new Direction[] 
            {
                Direction.up,
                Direction.down,
                Direction.right,
                Direction.left,
                Direction.forward,
                Direction.back
            }, Allocator.Persistent);

            _vertexOffsets = new NativeArray<float3>(new float3[]
            {
                new float3(-_VERTEX_OFFSET, -_VERTEX_OFFSET, -_VERTEX_OFFSET),
                new float3(-_VERTEX_OFFSET, _VERTEX_OFFSET, -_VERTEX_OFFSET),
                new float3(-_VERTEX_OFFSET, _VERTEX_OFFSET, _VERTEX_OFFSET),
                new float3(-_VERTEX_OFFSET, -_VERTEX_OFFSET, _VERTEX_OFFSET),
                new float3(_VERTEX_OFFSET, -_VERTEX_OFFSET, -_VERTEX_OFFSET),
                new float3(_VERTEX_OFFSET, _VERTEX_OFFSET, -_VERTEX_OFFSET),
                new float3(_VERTEX_OFFSET, _VERTEX_OFFSET, _VERTEX_OFFSET),
                new float3(_VERTEX_OFFSET, -_VERTEX_OFFSET, _VERTEX_OFFSET)
            }, Allocator.Persistent);
        }

        public void Dispose()
        {
            _directions.Dispose();
            _vertexOffsets.Dispose();
        }

        public MeshData GenerateMeshData(ChunkData chunkData)
        {
            MeshData meshData = new MeshData();
            NativeArray<ushort> voxelID = new NativeArray<ushort>(chunkData.voxels, Allocator.Persistent);

            GenerateMeshDataJob job = new GenerateMeshDataJob()
            {
                voxelDataStorage = _voxelDataStorage.Storage,
                voxelID = voxelID,
                directions = _directions,
                vertexOffsets = _vertexOffsets,
                vertices = meshData.vertices,
                triangles = meshData.triangles,
                uvs = meshData.uvs,
                chunkLength = chunkData.ChunkLength,
                chunkHeight = chunkData.ChunkHeight,
                tileSize = _tileSize,
                xStep = _xStep,
                yStep = _yStep
            };

            JobHandle jobHandle = job.Schedule();
            jobHandle.Complete();

            voxelID.Dispose();

            return meshData;
        }
    }

    [BurstCompile]
    public struct GenerateMeshDataJob : IJob
    {
        [ReadOnly] public NativeHashMap<int, TSVoxelData> voxelDataStorage;
        [ReadOnly] public NativeArray<ushort> voxelID;
        [ReadOnly] public NativeArray<Direction> directions;
        [ReadOnly] public NativeArray<float3> vertexOffsets;
        public NativeList<float3> vertices;
        public NativeList<int> triangles;
        public NativeList<float3> uvs;
        public byte chunkLength;
        public byte chunkHeight;

        public int tileSize;
        public float xStep;
        public float yStep;
        private const float _TEXTURE_OFFSET = 0.001f;

        public void Execute()
        {
            for (int x = 0; x < chunkLength; ++x)
                for (int y = 0; y < chunkHeight; ++y)
                    for (int z = 0; z < chunkLength; ++z)
                        GenerateVoxelFaces(new int3(x, y, z));
        }

        private void GenerateVoxelFaces(int3 localPosition)
        {
            int index = ChunkHandler.LocalPositionToIndex(chunkLength, chunkHeight, localPosition);

            voxelDataStorage.TryGetValue(voxelID[index], out var voxelData);
            VoxelType voxelType = voxelData.type;

            if (voxelType == VoxelType.Air || voxelType == VoxelType.Nothing)
                return;

            foreach (var direction in directions)
            {
                localPosition = ChunkHandler.IndexToLocalPosition(chunkLength, chunkHeight, index);
                int3 neigbourVoxelLocalPosition = localPosition + direction.ToInt3();
                int neighbourVoxelIndex = GetVoxelAt(neigbourVoxelLocalPosition);

                if (neighbourVoxelIndex == -1)
                    continue;

                VoxelType neighbourVoxelType = voxelDataStorage[voxelID[neighbourVoxelIndex]].type;

                if (neighbourVoxelType == VoxelType.Air || neighbourVoxelType == VoxelType.Transparent)
                    SetVoxelFace(voxelData, direction, localPosition);
            }
        }

        private void SetVoxelFace(TSVoxelData voxelData, Direction direction, int3 localPosition)
        {
            CreateQuad(direction, localPosition);
            AssignUVs(voxelData, direction);
        }

        private void CreateQuad(Direction direction, int3 localPosition)
        {
            switch (direction)
            {
                case Direction.up:
                    vertices.Add(localPosition + vertexOffsets[2]);
                    vertices.Add(localPosition + vertexOffsets[6]);
                    vertices.Add(localPosition + vertexOffsets[5]);
                    vertices.Add(localPosition + vertexOffsets[1]);
                    break;

                case Direction.down:
                    vertices.Add(localPosition + vertexOffsets[0]);
                    vertices.Add(localPosition + vertexOffsets[4]);
                    vertices.Add(localPosition + vertexOffsets[7]);
                    vertices.Add(localPosition + vertexOffsets[3]);
                    break;

                case Direction.right:
                    vertices.Add(localPosition + vertexOffsets[4]);
                    vertices.Add(localPosition + vertexOffsets[5]);
                    vertices.Add(localPosition + vertexOffsets[6]);
                    vertices.Add(localPosition + vertexOffsets[7]);
                    break;

                case Direction.left:
                    vertices.Add(localPosition + vertexOffsets[3]);
                    vertices.Add(localPosition + vertexOffsets[2]);
                    vertices.Add(localPosition + vertexOffsets[1]);
                    vertices.Add(localPosition + vertexOffsets[0]);
                    break;

                case Direction.forward:
                    vertices.Add(localPosition + vertexOffsets[7]);
                    vertices.Add(localPosition + vertexOffsets[6]);
                    vertices.Add(localPosition + vertexOffsets[2]);
                    vertices.Add(localPosition + vertexOffsets[3]);
                    break;

                case Direction.back:
                    vertices.Add(localPosition + vertexOffsets[0]);
                    vertices.Add(localPosition + vertexOffsets[1]);
                    vertices.Add(localPosition + vertexOffsets[5]);
                    vertices.Add(localPosition + vertexOffsets[4]);
                    break;
            }

            triangles.Add(vertices.Length - 4);
            triangles.Add(vertices.Length - 3);
            triangles.Add(vertices.Length - 2);

            triangles.Add(vertices.Length - 4);
            triangles.Add(vertices.Length - 2);
            triangles.Add(vertices.Length - 1);
        }

        private void AssignUVs(TSVoxelData voxelData, Direction direction)
        {
            if (voxelData.type == VoxelType.Air)
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
            uvs.Add(new float3(xStep * textureCoordinates.x + _TEXTURE_OFFSET, xStep * textureCoordinates.y + _TEXTURE_OFFSET, 0));
            uvs.Add(new float3(xStep * textureCoordinates.x + _TEXTURE_OFFSET, yStep * textureCoordinates.y + yStep - _TEXTURE_OFFSET, 0));
            uvs.Add(new float3(xStep * textureCoordinates.x + xStep - _TEXTURE_OFFSET, yStep * textureCoordinates.y + yStep - _TEXTURE_OFFSET, 0));
            uvs.Add(new float3(xStep * textureCoordinates.x + xStep - _TEXTURE_OFFSET, yStep * textureCoordinates.y + _TEXTURE_OFFSET, 0));
        }

        private int GetVoxelAt(int3 localPosition)
        {
            if (IsInBounds(localPosition))
                return ChunkHandler.LocalPositionToIndex(chunkLength, chunkHeight, localPosition);

            return -1;
        }

        private bool IsInBounds(int3 localPosition)
        {
            if (localPosition.x < 0 || localPosition.x >= chunkLength ||
                localPosition.y < 0 || localPosition.y >= chunkHeight ||
                localPosition.z < 0 || localPosition.z >= chunkLength)
                return false;

            return true;
        }
    }
}