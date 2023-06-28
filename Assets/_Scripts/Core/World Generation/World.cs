using HerosJourney.Core.WorldGeneration.Chunks;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections;
using System.Threading;
using Zenject;
using System.Collections.Concurrent;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        [SerializeField] private int _chunkLength = 16;
        [SerializeField] private int _chunkHeight = 128;
        [SerializeField, Range(1, 32)] private int _renderDistance = 8;

        [SerializeField] private WorldRenderer _worldRenderer;
        private TerrainGenerator _terrainGenerator;

        private CancellationTokenSource _taskTokenSource = new CancellationTokenSource();

        public Action OnNewChunksInitialized;

        public int ChunkLength => _chunkLength;
        public int ChunkHeight => _chunkHeight;
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

        [Inject]
        private void Construct(TerrainGenerator terrainGenerator)
        {
            _terrainGenerator = terrainGenerator;
        }

        public async void GenerateChunks() => await GenerateChunks(Vector3Int.zero);
        
        public async void GenerateChunksRequest(Vector3Int worldPosition) => await GenerateChunks(worldPosition);

        private async Task GenerateChunks(Vector3Int worldPosition)
        {
            WorldGenerationData worldGenerationData = await Task.Run(() => GetWorldGenerationData(worldPosition), _taskTokenSource.Token);

            RemoveDistantChunks(worldGenerationData);

            ConcurrentDictionary<Vector3Int, ChunkData> chunkDataDictionary = null;
            ConcurrentDictionary<Vector3Int, MeshData> meshDataDicitonary = null;

            try
            {
                chunkDataDictionary = await GenerateChunkData(worldGenerationData.chunkDataPositionsToCreate);
                foreach (var data in chunkDataDictionary)
                    WorldData.chunkData.Add(data.Key, data.Value);

                List<ChunkData> dataToRender = WorldDataHandler.SelectChunksToRender(WorldData, worldGenerationData.chunkRendererPositionsToCreate);
                meshDataDicitonary = await GenerateMeshData(dataToRender);
            } 
            catch (Exception)
            {
                return;
            }
            
            StartCoroutine(CreateChunks(meshDataDicitonary));
        }

        private WorldGenerationData GetWorldGenerationData(Vector3Int worldPosition)
        {
            List<Vector3Int> nearestChunkDataPositions = WorldDataHandler.GetChunksAroundPoint(WorldData, worldPosition, _renderDistance + 1);
            List<Vector3Int> nearestChunkRendererPositions = WorldDataHandler.GetChunksAroundPoint(WorldData, worldPosition, _renderDistance);

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

        private void RemoveDistantChunks(WorldGenerationData worldGenerationData)
        {
            foreach (Vector3Int position in worldGenerationData.chunkDataPositionsToRemove)
                WorldData.chunkData.Remove(position);

            foreach (Vector3Int position in worldGenerationData.chunkRendererPositionsToRemove)
            {
                _worldRenderer.UnloadChunk(WorldData.chunkRenderers[position]);

                WorldData.chunkRenderers.Remove(position);
            }
        }

        private Task<ConcurrentDictionary<Vector3Int, ChunkData>> GenerateChunkData(List<Vector3Int> chunkDataPositionsToCreate)
        {
            ConcurrentDictionary<Vector3Int, ChunkData> dictionary = new ConcurrentDictionary<Vector3Int, ChunkData>();

            return Task.Run(() =>
            {
                foreach (Vector3Int position in chunkDataPositionsToCreate)
                {
                    if (_taskTokenSource.IsCancellationRequested)
                        _taskTokenSource.Token.ThrowIfCancellationRequested();

                    ChunkData chunkData = new ChunkData(_chunkLength, _chunkHeight, position, this);
                    _terrainGenerator.GenerateChunkData(chunkData);

                    dictionary.TryAdd(position, chunkData);
                }

                return dictionary;
            }, _taskTokenSource.Token);
        }

        private Task<ConcurrentDictionary<Vector3Int, MeshData>> GenerateMeshData(List<ChunkData> chunkDataToRender)
        {
            ConcurrentDictionary<Vector3Int, MeshData> dictionary = new ConcurrentDictionary<Vector3Int, MeshData>();

            return Task.Run(() =>
            {
                foreach (ChunkData chunkData in chunkDataToRender)
                {
                    if (_taskTokenSource.Token.IsCancellationRequested)
                        _taskTokenSource.Token.ThrowIfCancellationRequested();

                    MeshData meshData = MeshDataBuilder.GenerateMeshData(chunkData);
                    dictionary.TryAdd(chunkData.WorldPosition, meshData);
                }

                return dictionary;
            }, _taskTokenSource.Token
            );
        }

        private IEnumerator CreateChunks(ConcurrentDictionary<Vector3Int, MeshData> meshDataDictionary)
        {
            foreach (var meshData in meshDataDictionary)
            {
                if (meshData.Value.ColliderTriangles.Count == 0)
                    continue;

                ChunkRenderer chunkRenderer = _worldRenderer.RenderChunk(WorldData.chunkData[meshData.Key], meshData.Value);
                WorldData.chunkRenderers.Add(meshData.Key, chunkRenderer);
                yield return new WaitForEndOfFrame();
            }

            OnNewChunksInitialized?.Invoke();
        }
    }
}