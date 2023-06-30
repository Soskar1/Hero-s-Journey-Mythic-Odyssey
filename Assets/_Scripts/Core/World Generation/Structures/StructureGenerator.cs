using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Noises;
using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Structures
{
    public class StructureGenerator : MonoBehaviour
    {
        [SerializeField] private NoiseSettings _noiseSettings;

        [SerializeField] private int _treeHeight;
        [SerializeField] private VoxelData _voxelData;
        private Voxel _voxel;

        private void Awake() => _voxel = new Voxel(_voxelData);

        public void GenerateStructures(ChunkData chunkData)
        {
            chunkData.structureData = GenerateStructureData(chunkData);
            PlaceStructures(chunkData);
        }

        private StructureData GenerateStructureData(ChunkData chunkData)
        {
            StructureData structureData = new StructureData();
            float[,] noise = Noise.GenerateNoise(chunkData.ChunkLength,
                new Vector2Int(chunkData.WorldPosition.x, chunkData.WorldPosition.z),
                _noiseSettings);

            structureData.structurePositions = Noise.FindLocalMaximas(noise);

            return structureData;
        }

        private void PlaceStructures(ChunkData chunkData)
        {
            foreach(Vector2Int position in chunkData.structureData.structurePositions)
            {
                Vector3Int localPosition = new Vector3Int(position.x, chunkData.groundHeight[position.x, position.y], position.y);

                for (int i = 0; i < _treeHeight; ++i)
                {
                    ++localPosition.y;
                    ChunkDataHandler.SetVoxelAt(chunkData, _voxel, localPosition);
                }
            }
        }
    }
}