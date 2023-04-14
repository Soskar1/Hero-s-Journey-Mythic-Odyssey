using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Voxels;
using HerosJourney.Core.WorldGeneration.Noises;
using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        [SerializeField] private int _chunkSize = 16;
        [SerializeField] private int _chunkHeight = 128;
        [SerializeField] private int _worldSizeInChunks;

        [SerializeField] private GameObject _chunkPrefab;
        [SerializeField] private NoiseSettings _noiseSettings;

        private Dictionary<Vector3Int, ChunkData> _chunks = new Dictionary<Vector3Int, ChunkData>();
        private Dictionary<Vector3Int, ChunkRenderer> _chunkRenderers = new Dictionary<Vector3Int, ChunkRenderer>();

        public void GenerateWorld()
        {
            ClearAllChunks();
            GenerateChunkData();
            RenderChunks();
        }

        private void ClearAllChunks()
        {
            _chunks.Clear();

            foreach (var chunk in _chunkRenderers.Values)
                Destroy(chunk.gameObject);

            _chunkRenderers.Clear();
        }

        private void GenerateChunkData()
        {
            for (int x = 0; x < _worldSizeInChunks; ++x)
            {
                for (int z = 0; z < _worldSizeInChunks; ++z)
                {
                    Vector3Int position = new Vector3Int(x * _chunkSize, 0, z * _chunkSize);
                    ChunkData chunkData = new ChunkData(_chunkSize, _chunkHeight, position, this);
                    GenerateVoxels(chunkData);
                    _chunks.Add(position, chunkData);
                }
            }
        }

        private void RenderChunks()
        {
            foreach (ChunkData chunkData in _chunks.Values)
            {
                MeshData meshData = ChunkVoxelData.GetChunkMeshData(chunkData);
                GameObject chunkInstance = Instantiate(_chunkPrefab, chunkData.WorldPosition, Quaternion.identity);
                ChunkRenderer chunkRenderer = chunkInstance.GetComponent<ChunkRenderer>();
                chunkRenderer.InitializeChunk(chunkData);
                chunkRenderer.UpdateChunk(meshData);

                _chunkRenderers.Add(chunkData.WorldPosition, chunkRenderer);
            }
        }

        private void GenerateVoxels(ChunkData data)
        {
            for (int x = 0; x < _chunkSize; ++x)
            {
                for (int z = 0; z < _chunkSize; ++z)
                {
                    float noise = Noise.OctavePerlinNoise(x + data.WorldPosition.x, z + data.WorldPosition.z, _noiseSettings);
                    int groundPosition = Mathf.RoundToInt(noise * _chunkHeight);

                    for (int y = 0; y < _chunkHeight; ++y)
                    {
                        VoxelType voxelType = VoxelType.Solid;

                        if (y > groundPosition)
                            voxelType = VoxelType.Air;

                        ChunkVoxelData.SetVoxelAt(data, new Voxel(voxelType), new Vector3Int(x, y, z));
                    }
                }
            }
        }

        public Voxel GetVoxelInWorld(Vector3Int worldPosition)
        {
            Vector3Int chunkPosition = GetChunkPosition(worldPosition);
            ChunkData chunk = null;

            if (_chunks.TryGetValue(chunkPosition, out chunk))
                return ChunkVoxelData.GetVoxelAt(chunk, ChunkVoxelData.WorldToLocalPosition(chunk, worldPosition));

            return null;
        }

        private Vector3Int GetChunkPosition(Vector3Int worldPosition)
        {
            return new Vector3Int
            {
                x = Mathf.FloorToInt(worldPosition.x / (float)_chunkSize) * _chunkSize,
                y = Mathf.FloorToInt(worldPosition.y / (float)_chunkHeight) * _chunkHeight,
                z = Mathf.FloorToInt(worldPosition.z / (float)_chunkSize) * _chunkSize
            };
        }
    }
}