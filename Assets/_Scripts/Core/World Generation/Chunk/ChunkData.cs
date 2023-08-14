using HerosJourney.Core.WorldGeneration.Voxels;
using HerosJourney.Core.WorldGeneration.Structures;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class ChunkData
    {
        public int[,,] voxelId;
        public int[,] groundHeight;
        public StructureData structureData;

        public int ChunkLength { get; private set; }
        public int ChunkHeight { get; private set; }
        public int ChunkSeed { get; private set; }
        public Vector3Int WorldPosition { get; private set; }

        public ChunkData (WorldData worldData, Vector3Int worldPosition)
        {
            ChunkLength = worldData.chunkLength;
            ChunkHeight = worldData.chunkHeight;
            ChunkSeed = worldData.worldSeed + worldPosition.x + worldPosition.y + worldPosition.z;
            voxelId = new int[worldData.chunkLength, worldData.chunkHeight, worldData.chunkLength];
            groundHeight = new int[worldData.chunkLength, worldData.chunkLength];
            WorldPosition = worldPosition;
        }
    }
}