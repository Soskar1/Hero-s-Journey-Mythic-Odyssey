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
        private VoxelStorage _voxelStorage;

        private List<VoxelSaveData> _structureVoxels = new List<VoxelSaveData>();

        [Inject]
        private void Construct(VoxelStorage voxelStorage) => _voxelStorage = voxelStorage;

        private void Start() => _structureVoxels = StructureSaveLoad.LoadStructure(_tree);

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
                    Voxel voxelInstance = _voxelStorage.GetVoxelByID(voxel.id);
                    ChunkDataHandler.SetVoxelAt(chunkData, voxelInstance, tmpPosition);
                    tmpPosition = localPosition;
                }
            }
        }
    }
}