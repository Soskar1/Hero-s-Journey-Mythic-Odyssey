using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class WorldData
    {
        public byte chunkLength;
        public byte chunkHeight;

        public Dictionary<Vector3Int, Chunk> existingChunks = new Dictionary<Vector3Int, Chunk>();

        public WorldData(byte chunkLength, byte chunkHeight)
        {
            this.chunkLength = chunkLength;
            this.chunkHeight = chunkHeight;
        }
    }
}