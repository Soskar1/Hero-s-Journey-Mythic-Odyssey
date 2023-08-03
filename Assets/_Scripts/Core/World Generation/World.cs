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
        [SerializeField] private int _worldSeed;
        [SerializeField] private int _chunkLength = 16;
        [SerializeField] private int _chunkHeight = 128;
        [SerializeField, Range(1, 32)] private int _renderDistance = 8;
        private WorldData _worldData;

        [SerializeField] private WorldRenderer _worldRenderer;
        private List<IGenerator> _generators;

        private CancellationTokenSource _taskTokenSource = new CancellationTokenSource();

        public Action OnNewChunksInitialized;

        public int ChunkLength => _chunkLength;
        public int ChunkHeight => _chunkHeight;
        public WorldData WorldData => _worldData;

        private struct WorldGenerationData
        {
            public List<Vector3Int> chunkDataPositionsToCreate;
            public List<Vector3Int> chunkRendererPositionsToCreate;

            public List<Vector3Int> chunkDataPositionsToRemove;
            public List<Vector3Int> chunkRendererPositionsToRemove;
        }

        [Inject]
        private void Construct(List<IGenerator> generators) => _generators = generators;
        private void OnEnable() => _worldData = new WorldData(_chunkLength, _chunkHeight, _worldSeed);
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
                await GenerateChunkData(worldGenerationData.chunkDataPositionsToCreate);
                List<ChunkData> dataToRender = WorldDataHandler.SelectChunksToRender(WorldData, worldGenerationData.chunkRendererPositionsToCreate, worldPosition);
                meshDataDictionary = await GenerateMeshData(dataToRender);
            }
            catch (Exception e) 
            {
                Debug.LogException(e);
                return;
            }
            
            StartCoroutine(RenderChunks(meshDataDictionary, worldGenerationData.chunkRendererPositionsToCreate));
        }

        private Task GenerateChunkData(List<Vector3Int> chunkDataPositionsToCreate)
        {
            ConcurrentDictionary<Vector3Int, ChunkData> chunkDataDictionary = AllocateMemoryForChunkData(chunkDataPositionsToCreate);
            
            foreach (var data in chunkDataDictionary)
                WorldData.chunkData.Add(data.Key, data.Value);

            return Task.Run(() => 
            {
                foreach (var generator in _generators)
                    foreach (ChunkData chunkData in chunkDataDictionary.Values)
                        generator.Generate(chunkData);
            });
        }

        private ConcurrentDictionary<Vector3Int, ChunkData> AllocateMemoryForChunkData(List<Vector3Int> chunkDataPositionsToCreate)
        {
            ConcurrentDictionary<Vector3Int, ChunkData> chunkDataDictionary = new ConcurrentDictionary<Vector3Int, ChunkData>();

            foreach (Vector3Int position in chunkDataPositionsToCreate)
            {
                ChunkData newChunkData = new ChunkData(ref _worldData, position);
                chunkDataDictionary.TryAdd(position, newChunkData);
            }

            return chunkDataDictionary;
        }

        private async Task<ConcurrentDictionary<Vector3Int, MeshData>> GenerateMeshData(List<Vector3Int> dataToRender)
        {
            ConcurrentDictionary<Vector3Int, MeshData> meshDataDicitonary = null;
            
            try
            {
                meshDataDicitonary = await GenerateMeshData(dataToRender);
            } 
            catch (Exception e)
            {
                throw e;
            }

            return meshDataDicitonary;
        }

        private WorldGenerationData GetWorldGenerationData(Vector3Int worldPosition)
        {
            List<Vector3Int> nearestChunkDataPositions = WorldDataHandler.GetChunksPositionsAroundPoint(WorldData, worldPosition, _renderDistance + 1);
            List<Vector3Int> nearestChunkRendererPositions = WorldDataHandler.GetChunksPositionsAroundPoint(WorldData, worldPosition, _renderDistance);

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