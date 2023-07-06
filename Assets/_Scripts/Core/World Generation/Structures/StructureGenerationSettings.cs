using HerosJourney.Core.WorldGeneration.Voxels;
using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Noises
{
    [CreateAssetMenu(fileName = "new Generation Settings", menuName = "World Generation/Structure Generation Settings")]
    public class StructureGenerationSettings : ScriptableObject
    {
        [Range(0f, 1f)] public float probability;
        [Range(0f, 1f)] public float threshold;
        [Range(1, 100)] public int radius;
        public List<VoxelData> voxelsNotToBuildOn;
    }
}