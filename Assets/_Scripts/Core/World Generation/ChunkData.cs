using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class ChunkData
    {
        public ushort[] voxels;

        public byte ChunkLength { get; private set; }
        public byte ChunkHeight { get; private set; }
        public Vector3Int WorldPosition { get; private set; }

        public ChunkData(WorldData worldData, Vector3Int worldPosition)
        {
            ChunkLength = worldData.chunkLength;
            ChunkHeight = worldData.chunkHeight;
            voxels = new ushort[worldData.chunkLength * worldData.chunkHeight * worldData.chunkLength];
            WorldPosition = worldPosition;
        }
    }
}