using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class WorldRenderer : MonoBehaviour
    {
        private Queue<ChunkRenderer> _chunkRenderers = new Queue<ChunkRenderer>();
    }
}