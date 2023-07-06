using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Noises
{
    [CreateAssetMenu(fileName = "new Noises Settings", menuName = "World Generation/Structure Noise Settings")]
    public class StructureNoiseSettings : NoiseSettings
    {
        public float probability;
    }
}