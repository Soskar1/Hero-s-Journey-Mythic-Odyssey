using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Biomes
{
    public abstract class LayerGenerator : MonoBehaviour
    {
        [SerializeField] private LayerGenerator _next;
        [SerializeField] private VoxelData _voxelData;

        public Voxel Voxel { get; private set; }

        private void Awake() => Voxel = new Voxel(_voxelData);

        public bool TryGenerateLayer(ChunkData chunkData, Vector3Int localPosition, int surfaceHeightNoise)
        {
            if (TryGenerateVoxels(chunkData, localPosition, surfaceHeightNoise))
                return true;

            if (_next != null)
                _next.TryGenerateLayer(chunkData, localPosition, surfaceHeightNoise);

            return false;
        }

        protected abstract bool TryGenerateVoxels(ChunkData chunkData, Vector3Int localPosition, int surfaceHeightNoise);
    }
}