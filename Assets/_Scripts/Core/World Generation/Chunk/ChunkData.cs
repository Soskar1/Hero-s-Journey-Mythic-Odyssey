using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class ChunkData
    {
        private int _chunkLength;
        private int _chunkHeight;
        public Voxel[,,] voxels;
        private Vector3Int _worldPosition;
        private World _world;

        public int ChunkLength => _chunkLength;
        public int ChunkHeight => _chunkHeight;
        public Vector3Int WorldPosition => _worldPosition;
        public World World => _world;

        public ChunkData (int chunkLength, int chunkHeight, Vector3Int worldPosition, World worldReference)
        {
            _chunkLength = chunkLength;
            _chunkHeight = chunkHeight;
            voxels = new Voxel[chunkLength, chunkHeight, chunkLength];
            _worldPosition = worldPosition;
            _world = worldReference;
        }
    }
}