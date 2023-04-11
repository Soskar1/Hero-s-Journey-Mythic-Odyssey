using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    [CreateAssetMenu(fileName = "Voxel", menuName = "World Generation/Voxel")]
    public class VoxelData : ScriptableObject
    {
        public VoxelType type;
    }
}