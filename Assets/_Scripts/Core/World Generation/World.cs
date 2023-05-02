using HerosJourney.Core.WorldGeneration.Chunks;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using UnityEngine.UI;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        [SerializeField] private Slider _renderDistanceSlider;
        [SerializeField] private int _chunkLength = 16;
        [SerializeField] private int _chunkHeight = 128;
        [SerializeField] [Range(4, 32)] 
        private int _renderDistance = 8;

        [SerializeField] private WorldRenderer _worldRenderer;
        [SerializeField] private TerrainGenerator _terrainGenerator;

        private CancellationTokenSource _taskTokenSource = new CancellationTokenSource();

        public Action OnNewChunksInitialized;

        public int ChunkLength => _chunkLength;
        public WorldData WorldData { get; private set; }

        private struct WorldGenerationData
        {
            public List<Vector3Int> chunkDataPositionsToCreate;
            public List<Vector3Int> chunkRendererPositionsToCreate;

            public List<Vector3Int> chunkDataPositionsToRemove;
            public List<Vector3Int> chunkRendererPositionsToRemove;
        }

        private void OnDisable() => _taskTokenSource.Cancel();
        private void Awake() => WorldData = new WorldData(_chunkLength, _chunkHeight);

        public void GenerateChunks() 
        {
            _renderDistance = (int)_renderDistanceSlider.value;
            GenerateChunks(Vector3Int.zero);
        }

        public async void GenerateChunks(Vector3Int worldPosition)
        {
            WorldGenerationData worldGenerationData = await Task.Run(() => GetWorldGenerationData(worldPosition), _taskTokenSource.Token);

            StartCoroutine(RemoveDistantChunks(worldGenerationData));

            try
            {
                await GenerateChunkData(worldGenerationData.chunkDataPositionsToCreate);
            } 
            catch (Exception)
            {
                return;
            }
            
            StartCoroutine(InitializeChunks(worldGenerationData.chunkRendererPositionsToCreate));
        }

        private WorldGenerationData GetWorldGenerationData(Vector3Int worldPosition)
        {
            List<Vector3Int> nearestChunkDataPositions = WorldDataHandler.GetChunkDataAroundPoint(WorldData, worldPosition, _renderDistance);
            List<Vector3Int> nearestChunkRendererPositions = WorldDataHandler.GetChunkRenderersAroundPoint(WorldData, worldPosition, _renderDistance);

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

        private IEnumerator RemoveDistantChunks(WorldGenerationData worldGenerationData)
        {
            foreach (Vector3Int position in worldGenerationData.chunkDataPositionsToRemove)
                WorldData.chunkData.Remove(position);

            foreach (Vector3Int position in worldGenerationData.chunkRendererPositionsToRemove)
            {
                _worldRenderer.UnloadChunk(WorldData.chunkRenderers[position]);

                WorldData.chunkRenderers.Remove(position);
                yield return new WaitForEndOfFrame();
            }
        }

        private Task GenerateChunkData(List<Vector3Int> chunkDataPositionsToCreate)
        {
            return Task.Run(() =>
            {
                foreach (Vector3Int position in chunkDataPositionsToCreate)
                {
                    if (_taskTokenSource.IsCancellationRequested)
                        _taskTokenSource.Token.ThrowIfCancellationRequested();

                    ChunkData chunkData = new ChunkData(_chunkLength, _chunkHeight, position, this);
                    _terrainGenerator.GenerateChunkData(chunkData);

                    WorldData.chunkData.Add(position, chunkData);
                }
            }, _taskTokenSource.Token);
        }

        private IEnumerator InitializeChunks(List<Vector3Int> chunkRendererPositionsToCreate)
        {
            foreach (Vector3Int position in chunkRendererPositionsToCreate)
            {
                ChunkData chunkData = WorldData.chunkData[position];
                MeshData meshData = ChunkDataHandler.GenerateMeshData(chunkData);
                ChunkRenderer chunkRenderer = _worldRenderer.RenderChunk(chunkData, meshData);

                WorldData.chunkRenderers.Add(chunkData.WorldPosition, chunkRenderer);
                yield return new WaitForEndOfFrame();
            }

            OnNewChunksInitialized?.Invoke();
        }
    }
}