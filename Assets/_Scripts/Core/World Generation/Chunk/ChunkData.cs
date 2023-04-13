using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class ChunkData
    {
        private int _chunkSize = 16;
        private int _chunkHeight = 128;
        public Voxel[,,] voxels;
        private Vector3Int _worldPosition;
        private World _world;

        public int ChunkSize => _chunkSize;
        public int ChunkHeight => _chunkHeight;
        public Vector3Int WorldPosition => _worldPosition;
        public World World => _world;

        public ChunkData (int chunkSize, int chunkHeight, Vector3Int worldPosition, World worldReference)
        {
            _chunkSize = chunkSize;
            _chunkHeight = chunkHeight;
            voxels = new Voxel[chunkSize, chunkHeight, chunkSize];
            _worldPosition = worldPosition;
            _world = worldReference;
        }
    }
}