using HerosJourney.Core.WorldGeneration.Chunks;
using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public struct WorldData
    {
        public int chunkLength;
        public int chunkHeight;
        public int worldSeed;

        public Dictionary<Vector3Int, ChunkData> chunkData;
        public Dictionary<Vector3Int, ChunkRenderer> chunkRenderers;

        public WorldData(int chunkLength, int chunkHeight, int worldSeed)
        {
            this.chunkLength = chunkLength;
            this.chunkHeight = chunkHeight;
            this.worldSeed = worldSeed;
            chunkData = new Dictionary<Vector3Int, ChunkData>();
            chunkRenderers = new Dictionary<Vector3Int, ChunkRenderer>();
        }
    }
}