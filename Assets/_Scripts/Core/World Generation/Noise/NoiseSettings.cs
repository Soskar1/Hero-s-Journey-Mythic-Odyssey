using Unity.Mathematics;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Noise
{
    [CreateAssetMenu(fileName = "new Noises Settings", menuName = "World Generation/Noise Settings")]
    public class NoiseSettings : ScriptableObject
    {
        public int octaves;
        public float persistence;
        public float noiseZoom;
        public int2 offset;
    }

    public struct TSNoiseSettings
    {
        public int octaves;
        public float persistence;
        public float noiseZoom;
        public int2 offset;

        public TSNoiseSettings(NoiseSettings noiseSettings)
        {
            octaves = noiseSettings.octaves;
            persistence = noiseSettings.persistence;
            noiseZoom = noiseSettings.noiseZoom;
            offset = noiseSettings.offset;
        }
    }
}