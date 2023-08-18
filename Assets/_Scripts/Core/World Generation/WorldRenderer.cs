using System.Collections.Generic;
using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class WorldRenderer : MonoBehaviour
    {
        private Queue<ChunkRenderer> _chunkRenderers = new Queue<ChunkRenderer>();
    }
}