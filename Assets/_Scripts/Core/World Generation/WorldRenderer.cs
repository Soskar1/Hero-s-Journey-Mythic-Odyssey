using System.Collections.Generic;
using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class WorldRenderer : MonoBehaviour
    {
        [SerializeField] private ChunkRenderer _chunkPrefab;
        [SerializeField] private int _capacity;

        private Queue<ChunkRenderer> _chunkRenderers;

        private void Awake() => _chunkRenderers = new Queue<ChunkRenderer>(_capacity);

        private void Start()
        {
            for (int i = 0; i < _capacity; ++i)
            {
                ChunkRenderer chunkInstance = Instantiate(_chunkPrefab);
                chunkInstance.transform.parent = transform;
                Enqueue(chunkInstance);
            }
        }

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
            }
            else
            {
                renderer = Instantiate(_chunkPrefab);
                renderer.transform.parent = transform;
            }

            renderer.gameObject.SetActive(true);
            return renderer;
        }
    }
}