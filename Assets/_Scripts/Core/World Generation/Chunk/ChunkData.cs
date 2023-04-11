using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class ChunkData
    {
        private int _chunkSize = 16;
        private int _chunkHeight = 128;
        public Voxel[,,] voxels;
        private Vector3Int _worldPosition;

        public int ChunkSize => _chunkSize;
        public int ChunkHeight => _chunkHeight;

        public ChunkData (int chunkSize, int chunkHeight, Vector3Int worldPosition)
        {
            _chunkSize = chunkSize;
            _chunkHeight = chunkHeight;
            voxels = new Voxel[chunkSize, chunkHeight, chunkSize];
            _worldPosition = worldPosition;
        }
    }
}