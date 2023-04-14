using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    [CreateAssetMenu(fileName = "Voxel", menuName = "World Generation/Voxel")]
    public class VoxelData : ScriptableObject
    {
        public VoxelType type;
    }
}