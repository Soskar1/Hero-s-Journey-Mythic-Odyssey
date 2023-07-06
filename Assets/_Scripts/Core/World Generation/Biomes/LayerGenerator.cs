using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Voxels;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration.Terrain
{
    public abstract class LayerGenerator : MonoBehaviour
    {
        [SerializeField] private LayerGenerator _next;
        [SerializeField] private VoxelData _voxelData;
        private VoxelStorage _voxelStorage;

        protected Voxel MainVoxel => _voxelStorage.Get(_voxelData);
        protected VoxelStorage VoxelStorage => _voxelStorage;

        [Inject]
        private void Construct(VoxelStorage voxelStorage)
        {
            _voxelStorage = voxelStorage;
        }

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