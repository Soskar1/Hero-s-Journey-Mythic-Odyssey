using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class World : MonoBehaviour
    {
        [SerializeField] private int _chunkSize = 16;
        [SerializeField] private int _chunkHeight = 128;
        [SerializeField] private float _noiseScale;

        [SerializeField] private GameObject _chunkPrefab;

        private void Start() => GenerateWorld();

        public void GenerateWorld()
        {
            ChunkData chunkData = new ChunkData(_chunkSize, _chunkHeight, Vector3Int.zero);
            GenerateVoxels(chunkData);

            MeshData meshData = ChunkVoxelData.GetChunkMeshData(chunkData);
            GameObject chunkInstance = Instantiate(_chunkPrefab, chunkData.WorldPosition, Quaternion.identity);
            ChunkRenderer chunkRenderer = chunkInstance.GetComponent<ChunkRenderer>();
            chunkRenderer.InitializeChunk(chunkData);
            chunkRenderer.UpdateChunk(meshData);
        }

        private void GenerateVoxels(ChunkData data)
        {
            for (int x = 0; x < _chunkSize; ++x)
            {
                for (int z = 0; z < _chunkSize; ++z)
                {
                    float noise = Mathf.PerlinNoise((data.WorldPosition.x + x) * _noiseScale, (data.WorldPosition.z + z) * _noiseScale);
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
    }
}