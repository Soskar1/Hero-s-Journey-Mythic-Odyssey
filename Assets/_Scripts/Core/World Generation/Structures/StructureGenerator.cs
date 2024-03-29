using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Noises;
using HerosJourney.Core.WorldGeneration.Voxels;
using HerosJourney.StructureBuilder.Saving;
using HerosJourney.Utils;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration.Structures
{
    public class StructureGenerator : MonoBehaviour, IGenerator
    {
        [SerializeField] private NoiseSettings _noiseSettings;
        [SerializeField] private StructureGenerationSettings _generationSettings;
        [SerializeField] private TextAsset _tree;
        private VoxelStorage _voxelStorage;
        private StructureStorage _structureStorage;
        private PointSelectionSettings _selectionSettings;

        private List<VoxelSaveData> _structureVoxels = new List<VoxelSaveData>();

        [Inject]
        private void Construct(VoxelStorage voxelStorage, StructureStorage structureStorage)
        {
            _voxelStorage = voxelStorage;
            _structureStorage = structureStorage;
        }

        public void Start() 
        {
            _structureVoxels = _structureStorage.Get(_tree.name);
            _selectionSettings = new PointSelectionSettings(_generationSettings.noiseThreshold, _generationSettings.radius);
        }

        public void Generate(ChunkData chunkData)
        {
            chunkData.structureData = GenerateStructureData(chunkData);
            PlaceStructures(chunkData);
        }

        private StructureData GenerateStructureData(ChunkData chunkData)
        {
            Noise.SetSeed(chunkData.ChunkSeed);

            StructureData structureData = new StructureData();
            float[,] noise = Noise.GenerateNoise(chunkData.ChunkLength,
                new Vector2Int(chunkData.WorldPosition.x, chunkData.WorldPosition.z),
                _noiseSettings);

            structureData.structurePositions = Noise.FindPointsAboveThreshold(noise, _selectionSettings,
                () => ThreadSafeRandom.NextDouble() <= _generationSettings.probability);

            return structureData;
        }

        private void PlaceStructures(ChunkData chunkData)
        {
            foreach (Vector2Int position in chunkData.structureData.structurePositions)
            {
                int localY = chunkData.groundHeight[position.x, position.y];
                if (localY >= _generationSettings.heightThreshold)
                    continue;

                Vector3Int localPosition = new Vector3Int(position.x, chunkData.groundHeight[position.x, position.y], position.y);
                PlaceStructureVoxels(chunkData, localPosition);
            }
        }

        private void PlaceStructureVoxels(ChunkData chunkData, Vector3Int localPosition)
        {
            Vector3Int tmpPosition = localPosition;

                foreach (var voxel in _structureVoxels)
                {
                    tmpPosition += voxel.pos;

                    Voxel currentVoxel = ChunkDataHandler.GetVoxelAt(chunkData, tmpPosition);
                    if (currentVoxel != null && _generationSettings.voxelsNotToBuildOn.Contains(currentVoxel.data))
                        break;

                    Voxel voxelInstance = _voxelStorage.GetVoxelByID(voxel.id);
                    ChunkDataHandler.SetVoxelAt(chunkData, voxelInstance, tmpPosition);
                    tmpPosition = localPosition;
                }
        }
    }
}