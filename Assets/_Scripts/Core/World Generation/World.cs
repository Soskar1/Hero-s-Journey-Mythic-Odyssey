using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
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

        private CancellationTokenSource _taskTokenSource = new CancellationTokenSource();

        public Action OnNewChunksInitialized;

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

        private void OnDisable() => _taskTokenSource.Cancel();

        public async void GenerateChunks() => await GenerateChunks(Vector3Int.zero);
        
        public async void GenerateChunksRequest(Vector3Int worldPosition) => await GenerateChunks(worldPosition);

        private async Task GenerateChunks(Vector3Int worldPosition)
        {
            WorldGenerationData worldGenerationData = await Task.Run(
                () => GetWorldGenerationData(worldPosition), _taskTokenSource.Token);

            UnloadChunks(worldGenerationData);

            ConcurrentDictionary<Vector3Int, MeshData> meshDataDictionary = null;
            try
            {
                await _chunkGenerator.GenerateChunkData(_worldGenerationSettings.WorldData, worldGenerationData.chunkDataPositionsToCreate);
                List<ChunkData> dataToRender = WorldDataHandler.SelectChunksToRender(WorldData, worldGenerationData.chunkRendererPositionsToCreate, worldPosition);
                meshDataDictionary = await _chunkGenerator.GenerateMeshData(dataToRender, _taskTokenSource);
            }
            catch (Exception e) 
            {
                Debug.LogException(e);
                return;
            }
            
            StartCoroutine(RenderChunks(meshDataDictionary, worldGenerationData.chunkRendererPositionsToCreate));
        }

        private WorldGenerationData GetWorldGenerationData(Vector3Int worldPosition)
        {
            List<Vector3Int> nearestChunkDataPositions = WorldDataHandler.GetChunksPositionsAroundPoint(WorldData, worldPosition, _worldGenerationSettings.RenderDistance + 1);
            List<Vector3Int> nearestChunkRendererPositions = WorldDataHandler.GetChunksPositionsAroundPoint(WorldData, worldPosition, _worldGenerationSettings.RenderDistance);

            List<Vector3Int> chunkDataPositionsToCreate = WorldDataHandler.SelectChunkDataPositionsToCreate(WorldData, nearestChunkDataPositions, worldPosition);
            List<Vector3Int> chunkRendererPositionsToCreate = WorldDataHandler.SelectChunkRendererPositionsToCreate(WorldData, nearestChunkRendererPositions, worldPosition);

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

        private IEnumerator RenderChunks(ConcurrentDictionary<Vector3Int, MeshData> meshDataDictionary, List<Vector3Int> dataToRender)
        {
            foreach (var pos in dataToRender)
            {
                if (meshDataDictionary[pos].ColliderTriangles.Count == 0)
                    continue;

                ChunkRenderer chunkRenderer = _worldRenderer.RenderChunk(WorldData.chunkData[pos], meshDataDictionary[pos]);
                WorldData.chunkRenderers.Add(pos, chunkRenderer);
                yield return new WaitForEndOfFrame(); 
            }

            OnNewChunksInitialized?.Invoke();
        }
    }
}