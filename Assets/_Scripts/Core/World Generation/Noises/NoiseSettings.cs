using Unity.Mathematics;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Noises
{
    [CreateAssetMenu(fileName = "new Noises Settings", menuName = "World Generation/Noise Settings")]
    public class NoiseSettings : ScriptableObject
    {
        public int octaves;
        public float persistence;
        public float noiseZoom;
        public float2 offset;
    }

    public struct TSNoiseSettings
    {
        public int octaves;
        public float persistence;
        public float noiseZoom;
        public float2 offset;

        public TSNoiseSettings(NoiseSettings noiseSettings)
        {
            octaves = noiseSettings.octaves;
            persistence = noiseSettings.persistence;
            noiseZoom = noiseSettings.noiseZoom;
            offset = noiseSettings.offset;
        }

        public static implicit operator TSNoiseSettings(NoiseSettings noiseSettings) => new TSNoiseSettings(noiseSettings);
    }
}