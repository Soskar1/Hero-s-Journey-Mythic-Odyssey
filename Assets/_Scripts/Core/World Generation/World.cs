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
        [SerializeField] [Range(4, 16)] 
        private int _renderDistance = 8;

        [SerializeField] private ChunkRenderer _chunkPrefab;
        [SerializeField] private TerrainGenerator _terrainGenerator;

        public Action OnNewChunksGenerated;

        public int ChunkLength => _chunkLength;
        public WorldData WorldData { get; private set; }

        private struct WorldGenerationData
        {
            public List<Vector3Int> chunkDataPositionsToCreate;
            public List<Vector3Int> chunkRendererPositionsToCreate;

            public List<Vector3Int> chunkDataPositionsToRemove;
            public List<Vector3Int> chunkRendererPositionsToRemove;
        }

        private void Awake() => WorldData = new WorldData(_chunkLength, _chunkHeight);

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

        private void RemoveDistantChunks(WorldGenerationData worldGenerationData)
        {
            foreach (Vector3Int position in worldGenerationData.chunkDataPositionsToRemove)
                WorldData.chunkData.Remove(position);

            foreach (Vector3Int position in worldGenerationData.chunkRendererPositionsToRemove)
            {
                Destroy(WorldData.chunkRenderers[position].gameObject);
                WorldData.chunkRenderers.Remove(position);
            }
        }

        private void GenerateChunkData(List<Vector3Int> chunkDataPositionsToCreate)
        {
            foreach (Vector3Int position in chunkDataPositionsToCreate)
            {
                ChunkData chunkData = new ChunkData(_chunkLength, _chunkHeight, position, this);
                _terrainGenerator.GenerateChunkData(chunkData);

                WorldData.chunkData.Add(position, chunkData);
            }
        }

        private void InitializeChunks(List<Vector3Int> chunkRendererPositionsToCreate)
        {
            foreach(Vector3Int position in chunkRendererPositionsToCreate)
            {
                ChunkData chunkData = WorldData.chunkData[position];
                MeshData meshData = ChunkDataHandler.GenerateMeshData(chunkData);
                ChunkRenderer chunkInstance = Instantiate(_chunkPrefab, chunkData.WorldPosition, Quaternion.identity);

                chunkInstance.InitializeChunk(chunkData);
                chunkInstance.UpdateChunk(meshData);

                WorldData.chunkRenderers.Add(chunkData.WorldPosition, chunkInstance);
            }
        }
    }
}