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
            public List<Vector3Int> chunkDataPositionsToCreate;
            public List<Vector3Int> chunkRendererPositionsToCreate;

            public List<Vector3Int> chunkDataPositionsToRemove;
            public List<Vector3Int> chunkRendererPositionsToRemove;
        }

        private void Awake() => _worldData = new WorldData(_chunkLength, _chunkHeight);

        public void GenerateChunks() => GenerateChunks(Vector3Int.zero);

        public void GenerateChunks(Vector3Int worldPosition)
        {
            WorldGenerationData worldGenerationData = GetWorldGenerationData(worldPosition);

            RemoveDistantChunks(worldGenerationData);
            GenerateChunkData(worldGenerationData.chunkDataPositionsToCreate);
            InitializeChunks(worldGenerationData.chunkRendererPositionsToCreate);

            OnNewChunksGenerated?.Invoke();
        }

        private WorldGenerationData GetWorldGenerationData(Vector3Int worldPosition)
        {
            List<Vector3Int> nearestChunkDataPositions = WorldDataHandler.GetChunkDataAroundPoint(_worldData, worldPosition, _renderDistance);
            List<Vector3Int> nearestChunkRendererPositions = WorldDataHandler.GetChunkRenderersAroundPoint(_worldData, worldPosition, _renderDistance);

            List<Vector3Int> chunkDataPositionsToCreate = WorldDataHandler.SelectChunkDataPositionsToCreate(_worldData, nearestChunkDataPositions, worldPosition);
            List<Vector3Int> chunkRendererPositionsToCreate = WorldDataHandler.SelectChunkRendererPositionsToCreate(_worldData, nearestChunkRendererPositions, worldPosition);

            List<Vector3Int> chunkDataPositionsToRemove = WorldDataHandler.ExcludeMatchingChunkDataPositions(_worldData, nearestChunkDataPositions);
            List<Vector3Int> chunkRendererPositionsToRemove = WorldDataHandler.ExcludeMatchingChunkRendererPositions(_worldData, nearestChunkRendererPositions);

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
                _worldData.chunkData.Remove(position);

            foreach (Vector3Int position in worldGenerationData.chunkRendererPositionsToRemove)
            {
                Destroy(_worldData.chunkRenderers[position].gameObject);
                _worldData.chunkRenderers.Remove(position);
            }
        }

        private void GenerateChunkData(List<Vector3Int> chunkDataPositionsToCreate)
        {
            foreach (Vector3Int position in chunkDataPositionsToCreate)
            {
                ChunkData chunkData = new ChunkData(_chunkLength, _chunkHeight, position, this);
                _terrainGenerator.GenerateChunkData(chunkData);

                _worldData.chunkData.Add(position, chunkData);
            }
        }

        private void InitializeChunks(List<Vector3Int> chunkRendererPositionsToCreate)
        {
            foreach(Vector3Int position in chunkRendererPositionsToCreate)
            {
                ChunkData chunkData = _worldData.chunkData[position];
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