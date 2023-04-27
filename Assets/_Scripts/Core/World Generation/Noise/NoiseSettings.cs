using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Noises
{
    [CreateAssetMenu(fileName = "new Noises Settings", menuName = "World Generation/Noises Settings")]
    public class NoiseSettings : ScriptableObject
    {
        public int octaves;
        public float persistence;
        public float noiseZoom;
        public Vector2 offset;
    }
}