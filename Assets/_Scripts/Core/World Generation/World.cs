using HerosJourney.Core.WorldGeneration.Chunks;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        [SerializeField] private int _chunkLength = 16;
        [SerializeField] private int _chunkHeight = 128;
        private WorldData _worldData;

        [SerializeField] [Range(4, 16)] 
        private int _renderDistance = 8;

        [SerializeField] private GameObject _chunkPrefab;

        [SerializeField] private TerrainGenerator _terrainGenerator;

        public Action OnNewChunksGenerated;

        public int ChunkLength => _chunkLength;
        public WorldData WorldData => _worldData;

        private struct WorldGenerationData
        {
            public List<Vector3Int> chunkPositionsToCreate;
            public List<Vector3Int> chunkPositionsToRemove;
        }

        private void Awake() => _worldData = new WorldData(_chunkLength, _chunkHeight);

        public void GenerateChunks() => GenerateChunks(Vector3Int.zero);

        public void GenerateChunks(Vector3Int worldPosition)
        {
            WorldGenerationData worldGenerationData = GetWorldGenerationData(worldPosition);

            RemoveDistantChunks(worldGenerationData.chunkPositionsToRemove);
            GenerateChunkData(worldGenerationData.chunkPositionsToCreate);
            InitializeChunks(worldGenerationData.chunkPositionsToCreate);

            OnNewChunksGenerated?.Invoke();
        }

        private WorldGenerationData GetWorldGenerationData(Vector3Int worldPosition)
        {
            List<Vector3Int> nearestChunkPositions = WorldDataHandler.GetChunksAroundPoint(_worldData, worldPosition, _renderDistance);
            List<Vector3Int> chunkPositionsToCreate = WorldDataHandler.SelectPositionsToCreate(_worldData, nearestChunkPositions, worldPosition);
            List<Vector3Int> chunkPositionsToRemove = WorldDataHandler.ExcludeMatchingChunkPositions(_worldData, nearestChunkPositions);

            WorldGenerationData worldGenerationData = new WorldGenerationData
            {
                chunkPositionsToCreate = chunkPositionsToCreate,
                chunkPositionsToRemove = chunkPositionsToRemove
            };

            return worldGenerationData;
        }

        private void RemoveDistantChunks(List<Vector3Int> chunkPositionsToRemove)
        {
            foreach (Vector3Int position in chunkPositionsToRemove)
            {
                _worldData.chunks.Remove(position);

                if (_worldData.chunkRenderers.TryGetValue(position, out ChunkRenderer chunkRenderer))
                {
                    Destroy(chunkRenderer.gameObject);
                    _worldData.chunkRenderers.Remove(position);
                }
            }
        }

        private void GenerateChunkData(List<Vector3Int> chunkPositionsToCreate)
        {
            foreach (Vector3Int position in chunkPositionsToCreate)
            {
                ChunkData chunkData = new ChunkData(_chunkLength, _chunkHeight, position, this);
                _terrainGenerator.GenerateChunkData(chunkData);

                _worldData.chunks.Add(position, chunkData);
            }
        }

        private void InitializeChunks(List<Vector3Int> chunkPositionsToCreate)
        {
            foreach(Vector3Int position in chunkPositionsToCreate)
            {
                ChunkData chunkData = _worldData.chunks[position];
                MeshData meshData = ChunkDataHandler.GenerateMeshData(chunkData);
                GameObject chunkInstance = Instantiate(_chunkPrefab, chunkData.WorldPosition, Quaternion.identity);

                ChunkRenderer chunkRenderer = chunkInstance.GetComponent<ChunkRenderer>();
                chunkRenderer.InitializeChunk(chunkData);
                chunkRenderer.UpdateChunk(meshData);

                _worldData.chunkRenderers.Add(chunkData.WorldPosition, chunkRenderer);
            }
        }
    }
}