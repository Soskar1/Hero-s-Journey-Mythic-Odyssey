using HerosJourney.Core.WorldGeneration.Voxels;
using HerosJourney.Core.WorldGeneration.Structures;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class ChunkData
    {
        public Voxel[,,] voxels;
        public StructureData structureData;

        public int ChunkLength { get; private set; }
        public int ChunkHeight { get; private set; }
        public Vector3Int WorldPosition { get; private set; }
        public World World { get; private set; }

        public ChunkData (int chunkLength, int chunkHeight, Vector3Int worldPosition, World worldReference)
        {
            ChunkLength = chunkLength;
            ChunkHeight = chunkHeight;
            voxels = new Voxel[chunkLength, chunkHeight, chunkLength];
            WorldPosition = worldPosition;
            World = worldReference;
        }
    }
}