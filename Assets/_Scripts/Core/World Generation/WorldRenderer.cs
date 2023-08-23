using System.Collections.Generic;
using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class WorldRenderer : MonoBehaviour
    {
        [SerializeField] private ChunkRenderer _chunkPrefab;

        private Queue<ChunkRenderer> _chunkRenderers = new Queue<ChunkRenderer>();

        public void Enqueue(ChunkRenderer renderer)
        {
            renderer.gameObject.SetActive(false);
            _chunkRenderers.Enqueue(renderer);
        }

        public ChunkRenderer Dequeue()
        {
            ChunkRenderer renderer;

            if (_chunkRenderers.Count > 0)
            {
                renderer = _chunkRenderers.Dequeue();
                renderer.gameObject.SetActive(true);
            }
            else
            {
                renderer = Instantiate(_chunkPrefab, transform.position, Quaternion.identity);
                renderer.transform.parent = transform;
            }

            return renderer;
        }
    }
}