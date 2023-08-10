using UnityEngine;
using HerosJourney.Core.WorldGeneration.Chunks;
using System.Collections.Generic;

namespace HerosJourney.Core.WorldGeneration
{
    public class WorldRenderer : MonoBehaviour
    {
        [SerializeField] private ChunkRenderer _chunkPrefab;
        private Queue<ChunkRenderer> _chunkPool = new Queue<ChunkRenderer>();

        public ChunkRenderer RenderChunk(Chunk chunk)
        {
            ChunkRenderer chunkRenderer;

            if (_chunkPool.Count > 0)
            {
                chunkRenderer = _chunkPool.Dequeue();
                chunkRenderer.gameObject.SetActive(true);
                chunkRenderer.transform.position = chunk.WorldPosition;
            }
            else
            {
                chunkRenderer = Instantiate(_chunkPrefab, chunk.WorldPosition, Quaternion.identity);
                chunkRenderer.transform.parent = transform;
            }

            chunkRenderer.InitializeChunk(chunk.chunkData);
            chunkRenderer.UpdateChunk(chunk.meshData);

            return chunkRenderer;
        }

        public void UnloadChunk(ChunkRenderer chunkRenderer)
        {
            _chunkPool.Enqueue(chunkRenderer);
            chunkRenderer.gameObject.SetActive(false);
        }
    }
}