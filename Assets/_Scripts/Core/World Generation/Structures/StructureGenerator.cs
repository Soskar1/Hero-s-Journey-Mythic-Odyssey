using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Noises;
using HerosJourney.Core.WorldGeneration.Voxels;
using HerosJourney.Core.WorldGeneration.Structures.Builder;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration.Structures
{
    public class StructureGenerator : MonoBehaviour
    {
        [SerializeField] private NoiseSettings _noiseSettings;
        [SerializeField] private TextAsset _tree;
        [SerializeField] private List<VoxelData> _voxelsNotToBuildOn;
        private VoxelStorage _voxelStorage;
        private StructureStorage _structureStorage;

        private List<VoxelSaveData> _structureVoxels = new List<VoxelSaveData>();

        [Inject]
        private void Construct(VoxelStorage voxelStorage, StructureStorage structureStorage)
        {
            _voxelStorage = voxelStorage;
            _structureStorage = structureStorage;
        }

        public void Start() => _structureVoxels = _structureStorage.Get(_tree.name);

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
            foreach (Vector2Int position in chunkData.structureData.structurePositions)
            {
                Vector3Int localPosition = new Vector3Int(position.x, chunkData.groundHeight[position.x, position.y], position.y);
                Vector3Int tmpPosition = localPosition;

                foreach (var voxel in _structureVoxels)
                {
                    tmpPosition += voxel.position;

                    Voxel currentVoxel = ChunkDataHandler.GetVoxelAt(chunkData, tmpPosition);
                    if (currentVoxel != null && _voxelsNotToBuildOn.Contains(currentVoxel.data))
                        break;

                    Voxel voxelInstance = _voxelStorage.GetVoxelByID(voxel.id);
                    ChunkDataHandler.SetVoxelAt(chunkData, voxelInstance, tmpPosition);
                    tmpPosition = localPosition;
                }
            }
        }
    }
}