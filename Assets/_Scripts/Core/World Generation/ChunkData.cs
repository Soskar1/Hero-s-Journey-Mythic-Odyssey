using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class ChunkData
    {
        private int _chunkSize = 16;
        private int _chunkHeight = 256;
        private Voxel[,,] _voxels;
        private Vector3Int _worldPosition;

        public ChunkData (int chunkSize, int chunkHeight, Vector3Int worldPosition)
        {
            _chunkSize = chunkSize;
            _chunkHeight = chunkHeight;
            _voxels = new Voxel[chunkSize, chunkHeight, chunkSize];
            _worldPosition = worldPosition;
        }
    }
}