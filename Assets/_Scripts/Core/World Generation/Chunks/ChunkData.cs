using System;
using Unity.Collections;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class ChunkData
    {
        private ushort[] _voxels;

        public byte Length { get; private set; }
        public byte Height { get; private set; }
        public ushort[] Voxels => _voxels;
        public int3 WorldPosition { get; private set; }

        public ChunkData(WorldData worldData, int3 worldPosition)
        {
            Length = worldData.ChunkLength;
            Height = worldData.ChunkHeight;
            _voxels = new ushort[Length * Length * Height];
            WorldPosition = worldPosition;
        }
    }

    public struct TSChunkData : IDisposable
    {
        public NativeArray<ushort> voxels;
        public byte Length { get; private set; }
        public byte Height { get; private set; }
        public int3 WorldPosition { get; private set; }

        public TSChunkData(ChunkData chunkData)
        {
            voxels = new NativeArray<ushort>(chunkData.Voxels, Allocator.TempJob);
            Length = chunkData.Length;
            Height = chunkData.Height;
            WorldPosition = chunkData.WorldPosition;
        }

        public void Dispose() => voxels.Dispose();

        public static implicit operator TSChunkData(ChunkData chunkData) => new TSChunkData(chunkData);
    }
}