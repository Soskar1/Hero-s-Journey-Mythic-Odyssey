using HerosJourney.Core.WorldGeneration.Voxels;
using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Noises
{
    [CreateAssetMenu(fileName = "new Generation Settings", menuName = "World Generation/Structure Generation Settings")]
    public class StructureGenerationSettings : ScriptableObject
    {
        public float probability;
        public float threshold;
        public List<VoxelData> voxelsNotToBuildOn;
    }
}