using HerosJourney.Core.WorldGeneration.Voxels;
using HerosJourney.Utils;
using System;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Zenject;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class MeshDataBuilder : IInitializable, IDisposable
    {
        private const float _VERTEX_OFFSET = 0.5f;
        private readonly VoxelDataTSStorage _voxelDataStorage;
        private NativeArray<Direction> _directions;
        private NativeArray<float3> _vertexOffsets;

        public MeshDataBuilder(VoxelDataTSStorage voxelDataStorage) => _voxelDataStorage = voxelDataStorage;

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

        public static MeshData GenerateMeshData(ChunkData chunkData)
        {
            MeshData meshData = new MeshData();

            //for (int x = 0; x < chunkData.ChunkLength; ++x)
            //    for (int y = 0; y < chunkData.ChunkHeight; ++y)
            //        for (int z = 0; z < chunkData.ChunkLength; ++z)
            //            GenerateVoxelFaces(chunkData, meshData, chunkData.voxels[x, y, z].data, new Vector3Int(x, y, z));

            return meshData;
        }
    }

    public struct GenerateMeshDataJob : IJobParallelFor
    {
        [ReadOnly] public NativeHashMap<int, TSVoxelData> voxelDataStorage;
        [ReadOnly] public NativeArray<ushort> voxelID;
        [ReadOnly] public NativeArray<Direction> directions;
        [ReadOnly] public NativeArray<int3> vertexOffsets;
        public NativeList<float3> vertices;
        public NativeList<int> triangles;
        public NativeList<float3> uvs;
        public byte chunkLength;
        public byte chunkHeight;

        public void Execute(int index)
        {
            voxelDataStorage.TryGetValue(voxelID[index], out var voxelData);
            VoxelType voxelType = voxelData.type;

            if (voxelType == VoxelType.Air || voxelType == VoxelType.Nothing)
                return;

            foreach (var direction in directions)
            {
                int3 localPosition = ChunkHandler.IndexToLocalPosition(index);
                int3 neigbourVoxelLocalPosition = localPosition + direction.ToInt3();
                int neighbourVoxelIndex = GetVoxelAt(neigbourVoxelLocalPosition);

                if (neighbourVoxelIndex == -1)
                    continue;

                VoxelType neighbourVoxelType = voxelDataStorage[neighbourVoxelIndex].type;

                if (neighbourVoxelType == VoxelType.Air || neighbourVoxelType == VoxelType.Transparent)
                    SetVoxelFace(voxelData, direction, localPosition);
            }
        }

        private void SetVoxelFace(TSVoxelData voxelData, Direction direction, int3 localPosition)
        {

        }

        private void CreateQuad(Direction direction)
        {


            triangles.Add(vertices.Length - 4);
            triangles.Add(vertices.Length - 3);
            triangles.Add(vertices.Length - 2);

            triangles.Add(vertices.Length - 4);
            triangles.Add(vertices.Length - 2);
            triangles.Add(vertices.Length - 1);
        }

        private int GetVoxelAt(int3 localPosition)
        {
            if (IsInBounds(localPosition))
                return ChunkHandler.LocalPositionToIndex(localPosition);

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