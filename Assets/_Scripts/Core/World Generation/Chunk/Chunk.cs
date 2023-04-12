using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class Chunk : MonoBehaviour
    {
        public ChunkData ChunkData { get; private set; }

        public void InitializeChunk(ChunkData data) => ChunkData = data;
    }
}