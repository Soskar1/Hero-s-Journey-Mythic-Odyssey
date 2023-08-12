using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Zenject;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        [SerializeField] private WorldRenderer _worldRenderer;

        private WorldGenerationSettings _worldGenerationSettings;
        private ChunkGenerator _chunkGenerator;

        public Action OnNewChunksInitialized;

        private ConcurrentQueue<Chunk> _chunksToRender = new ConcurrentQueue<Chunk>();

        public int ChunkLength => _worldGenerationSettings.WorldData.chunkLength;
        public int ChunkHeight => _worldGenerationSettings.WorldData.chunkHeight;
        public WorldData WorldData => _worldGenerationSettings.WorldData;

        private struct WorldGenerationData
        {
            public List<Vector3Int> chunkDataPositionsToCreate;
            public List<Vector3Int> chunkRendererPositionsToCreate;

            public List<Vector3Int> chunkDataPositionsToRemove;
            public List<Vector3Int> chunkRendererPositionsToRemove;
        }
        
        [Inject]
        private void Construct(WorldGenerationSettings worldGenerationSettings, ChunkGenerator chunkGenerator)
        {
            _chunkGenerator = chunkGenerator;
            _worldGenerationSettings = worldGenerationSettings;
        }

        private void OnEnable() => _chunkGenerator.ChunkGenerated += AddToRenderQueue;
        private void OnDisable() => _chunkGenerator.ChunkGenerated -= AddToRenderQueue;
        private void AddToRenderQueue(Chunk chunk) => _chunksToRender.Enqueue(chunk);

        private void Update()
        {
            if (_chunksToRender.IsEmpty)
                return;

            Timer.Start(0.1f, () => {
                _chunksToRender.TryDequeue(out Chunk chunk);
                ChunkRenderer renderer = _worldRenderer.RenderChunk(chunk);
                WorldData.chunkRenderers.Add(chunk.WorldPosition, renderer);
            });
        }

        public async void GenerateChunks() => await GenerateChunks(Vector3Int.zero);
        
        public async void GenerateChunksRequest(Vector3Int worldPosition) => await GenerateChunks(worldPosition);

        private async Task GenerateChunks(Vector3Int worldPosition)
        {
            WorldGenerationData worldGenerationData = await Task.Run(
                () => GetWorldGenerationData(worldPosition));

            UnloadChunks(worldGenerationData);

            try
            {
                await _chunkGenerator.GenerateChunkData(WorldData, worldGenerationData.chunkDataPositionsToCreate);
                List<ChunkData> dataToRender = WorldDataHandler.SelectChunksToRender(WorldData, worldGenerationData.chunkRendererPositionsToCreate, worldPosition);
                await _chunkGenerator.GenerateMeshData(dataToRender);
            }
            catch (Exception e) 
            {
                Debug.LogException(e);
                return;
            }

            OnNewChunksInitialized?.Invoke();
        }

        private WorldGenerationData GetWorldGenerationData(Vector3Int worldPosition)
        {
            List<Vector3Int> nearestChunkDataPositions = WorldDataHandler.GetChunksPositionsAroundPoint(WorldData, worldPosition, _worldGenerationSettings.RenderDistance + 1);
            List<Vector3Int> nearestChunkRendererPositions = WorldDataHandler.GetChunksPositionsAroundPoint(WorldData, worldPosition, _worldGenerationSettings.RenderDistance);

            List<Vector3Int> chunkDataPositionsToCreate = WorldDataHandler.SelectChunkDataPositionsToCreate(WorldData, nearestChunkDataPositions);
            List<Vector3Int> chunkRendererPositionsToCreate = WorldDataHandler.SelectChunkRendererPositionsToCreate(WorldData, nearestChunkRendererPositions);

            List<Vector3Int> chunkDataPositionsToRemove = WorldDataHandler.ExcludeMatchingChunkDataPositions(WorldData, nearestChunkDataPositions);
            List<Vector3Int> chunkRendererPositionsToRemove = WorldDataHandler.ExcludeMatchingChunkRendererPositions(WorldData, nearestChunkRendererPositions);

            WorldGenerationData worldGenerationData = new WorldGenerationData
            {
                chunkDataPositionsToCreate = chunkDataPositionsToCreate,
                chunkRendererPositionsToCreate = chunkRendererPositionsToCreate,

                chunkDataPositionsToRemove = chunkDataPositionsToRemove,
                chunkRendererPositionsToRemove = chunkRendererPositionsToRemove
            };
            
            return worldGenerationData;
        }

        private void UnloadChunks(WorldGenerationData worldGenerationData)
        {
            foreach (Vector3Int position in worldGenerationData.chunkDataPositionsToRemove)
                WorldData.chunkData.Remove(position);

            foreach (Vector3Int position in worldGenerationData.chunkRendererPositionsToRemove)
            {
                _worldRenderer.UnloadChunk(WorldData.chunkRenderers[position]);
                WorldData.chunkRenderers.Remove(position);
            }
        }
    }
}