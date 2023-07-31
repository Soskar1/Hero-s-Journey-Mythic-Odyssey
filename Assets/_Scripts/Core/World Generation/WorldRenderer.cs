using UnityEngine;
using HerosJourney.Core.WorldGeneration.Chunks;
using System.Collections.Generic;

namespace HerosJourney.Core.WorldGeneration
{
    public class WorldRenderer : MonoBehaviour
    {
        [SerializeField] private ChunkRenderer _chunkPrefab;
        private Queue<ChunkRenderer> _chunkPool;

        private void Awake() => _chunkPool = new Queue<ChunkRenderer>();

        public ChunkRenderer RenderChunk(ChunkData chunkData, MeshData meshData)
        {
            ChunkRenderer chunkRenderer;

            if (_chunkPool.Count > 0)
            {
                chunkRenderer = _chunkPool.Dequeue();
                chunkRenderer.gameObject.SetActive(true);
                chunkRenderer.transform.position = chunkData.WorldPosition;
            }
            else
            {
                chunkRenderer = Instantiate(_chunkPrefab, chunkData.WorldPosition, Quaternion.identity);
                chunkRenderer.transform.parent = transform;
            }

            chunkRenderer.InitializeChunk(chunkData);
            chunkRenderer.UpdateChunk(meshData);

            return chunkRenderer;
        }

        public void UnloadChunk(ChunkRenderer chunkRenderer)
        {
            _chunkPool.Enqueue(chunkRenderer);
            chunkRenderer.gameObject.SetActive(false);
        }
    }
}