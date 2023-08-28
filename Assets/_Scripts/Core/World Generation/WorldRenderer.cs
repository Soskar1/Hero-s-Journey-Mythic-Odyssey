using System.Collections.Generic;
using System.Linq;
using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class WorldRenderer : MonoBehaviour
    {
        [SerializeField] private ChunkRenderer _chunkPrefab;
        private Queue<ChunkRenderer> _pool = new Queue<ChunkRenderer>();

        public void Enqueue(ChunkRenderer chunk) => _pool.Enqueue(chunk);

        public ChunkRenderer Dequeue()
        {
            ChunkRenderer chunk;

            if (_pool.Any())
                chunk = _pool.Dequeue();
            else
                chunk = Instantiate(_chunkPrefab);

            return chunk;
        }
    }
}