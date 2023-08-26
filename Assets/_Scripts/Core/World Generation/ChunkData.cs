using Unity.Collections;

namespace HerosJourney.Core.WorldGeneration
{
    public class ChunkData
    {
        private VoxelType[] _voxels;

        public byte Lenght { get; private set; }
        public byte Height { get; private set; }
        public VoxelType[] Voxels => _voxels;

        public ChunkData(WorldData worldData)
        {
            Lenght = worldData.ChunkLength;
            Height = worldData.ChunkHeight;
            _voxels = new VoxelType[Lenght * Lenght * Height];
        }
    }

    public struct TSChunkData
    {
        public NativeArray<VoxelType> voxels;
        public byte Length { get; private set; }
        public byte Height { get; private set; }

        public TSChunkData(ChunkData chunkData)
        {
            voxels = new NativeArray<VoxelType>(chunkData.Voxels, Allocator.TempJob);
            Length = chunkData.Lenght;
            Height = chunkData.Height;
        }
    }
}