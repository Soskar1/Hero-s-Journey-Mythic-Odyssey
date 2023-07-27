using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Terrain;
using HerosJourney.Core.WorldGeneration.Structures;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Zenject;
using UnityEngine.UI;
using TMPro;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _seedInputField;
        [SerializeField] private Slider _renderDistanceSlider;
        [SerializeField] private int _chunkLength = 16;
        [SerializeField] private int _chunkHeight = 128;
        private int _worldSeed = 0; 
        private int _renderDistance = 8;
        private WorldData _worldData;

        [SerializeField] private WorldRenderer _worldRenderer;
        private TerrainGenerator _terrainGenerator;
        private StructureGenerator _structureGenerator;

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

        private void OnDisable() => _taskTokenSource.Cancel();

        [Inject]
        private void Construct(TerrainGenerator terrainGenerator, StructureGenerator structureGenerator)
        {
            _terrainGenerator = terrainGenerator;
            _structureGenerator = structureGenerator;

            _worldData = new WorldData(_chunkLength, _chunkHeight, _worldSeed);
        }

        public async void GenerateChunks() 
        {
            _renderDistance = (int)_renderDistanceSlider.value;
            _worldSeed = _seedInputField.text.Length == 0 ? 0 : Int32.Parse(_seedInputField.text);
            Debug.Log(_worldSeed);
            await GenerateChunks(Vector3Int.zero);
        }

        public async void GenerateChunksRequest(Vector3Int worldPosition) => await GenerateChunks(worldPosition);

        private async Task GenerateChunks(Vector3Int worldPosition)
        {
            WorldGenerationData worldGenerationData = await Task.Run(
                () => GetWorldGenerationData(worldPosition), _taskTokenSource.Token);

            RemoveDistantChunks(worldGenerationData);

            ConcurrentDictionary<Vector3Int, MeshData> meshDataDictionary = null;
            try
            {
                await GenerateChunkData(worldGenerationData.chunkDataPositionsToCreate);
                meshDataDictionary = await GenerateMeshData(worldGenerationData.chunkRendererPositionsToCreate);
            }
            catch (Exception e) 
            {
                Debug.LogException(e);
                return;
            }
            
            StartCoroutine(CreateChunks(meshDataDictionary));
        }

        private async Task GenerateChunkData(List<Vector3Int> chunkDataPositionsToCreate)
        {
            ConcurrentDictionary<Vector3Int, ChunkData> chunkDataDictionary = null;

            try
            {
                chunkDataDictionary = await GenerateTerrain(chunkDataPositionsToCreate);
            } 
            catch (Exception e)
            {
                throw e;
            }

            foreach (var data in chunkDataDictionary)
                WorldData.chunkData.Add(data.Key, data.Value);

            foreach (ChunkData chunkData in chunkDataDictionary.Values)
                _structureGenerator.GenerateStructures(chunkData);
        }

        private async Task<ConcurrentDictionary<Vector3Int, MeshData>> GenerateMeshData(List<Vector3Int> chunkRendererPositionsToCreate)
        {
            ConcurrentDictionary<Vector3Int, MeshData> meshDataDicitonary = null;
            List<ChunkData> dataToRender = WorldDataHandler.SelectChunksToRender(WorldData, chunkRendererPositionsToCreate);
            
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

        private Task<ConcurrentDictionary<Vector3Int, ChunkData>> GenerateTerrain(List<Vector3Int> chunkDataPositionsToCreate)
        {
            ConcurrentDictionary<Vector3Int, ChunkData> dictionary = new ConcurrentDictionary<Vector3Int, ChunkData>();

            return Task.Run(() =>
            {
                foreach (Vector3Int position in chunkDataPositionsToCreate)
                {
                    if (_taskTokenSource.IsCancellationRequested)
                        _taskTokenSource.Token.ThrowIfCancellationRequested();

                    ChunkData chunkData = new ChunkData(ref _worldData, position);
                    _terrainGenerator.GenerateTerrain(chunkData);

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