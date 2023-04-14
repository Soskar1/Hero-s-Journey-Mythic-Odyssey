using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Noises
{
    [CreateAssetMenu(fileName = "new Noises Settings", menuName = "World Generation/Noises Settings")]
    public class NoiseSettings : ScriptableObject
    {
        public int octaves;
        public float amplitude;
        public float frequency;
        public float persistence;
        public float size;
        public Vector2 offset;
    }
}