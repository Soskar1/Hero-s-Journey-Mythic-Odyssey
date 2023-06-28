using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Biomes
{
    public class TreeLayerGenerator : LayerGenerator
    {
        [SerializeField] private int _terrainHeightLimit = 50;
        [SerializeField] private int _treeHeight = 5;

        protected override bool TryGenerateVoxels(ChunkData chunkData, Vector3Int localPosition, int surfaceHeightNoise)
        {
            if (surfaceHeightNoise < _terrainHeightLimit &&
                chunkData.structureData.structurePositions.Contains(new Vector2Int(chunkData.WorldPosition.x + localPosition.x, chunkData.WorldPosition.z + localPosition.z)))
            {
                Vector3Int treePosition = new Vector3Int(localPosition.x, surfaceHeightNoise, localPosition.z);
                VoxelType overheadType = ChunkDataHandler.GetVoxelAt(chunkData, treePosition + Vector3Int.up).VoxelType;

                if (overheadType == VoxelType.Air)
                {
                    for (int i = 1; i < _treeHeight; ++i)
                    {
                        treePosition.y = surfaceHeightNoise + i;
                        ChunkDataHandler.SetVoxelAt(chunkData, MainVoxel, treePosition);
                    }

                    return true;
                }
            }

            return false;
        }
    }
}