using HerosJourney.Core.WorldGeneration.Noises;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class Noise
    {
        private readonly ThreadSafeNoiseSettings _noiseSettings;

        public Noise(ThreadSafeNoiseSettings noiseSettings) => _noiseSettings = noiseSettings;

        public float OctavePerlinNoise(float x, float y)
        {
            x *= _noiseSettings.noiseZoom;
            y *= _noiseSettings.noiseZoom;

            float total = 0;
            float maxValue = 0;

            float amplitude = 1;
            float frequency = 1;

            for (int i = 0; i < _noiseSettings.octaves; ++i)
            {
                total += Mathf.PerlinNoise((x + _noiseSettings.offset.x) * frequency, (y + _noiseSettings.offset.y) * frequency) * amplitude;

                maxValue += amplitude;

                amplitude *= _noiseSettings.persistence;
                frequency *= 2;
            }

            return total / maxValue;
        }
    }
}