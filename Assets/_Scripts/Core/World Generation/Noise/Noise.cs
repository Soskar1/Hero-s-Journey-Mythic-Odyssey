using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Noise
{
    public class Noise
    {
        private readonly TSNoiseSettings _noiseSettings;

        public Noise(TSNoiseSettings noiseSettings) => _noiseSettings = noiseSettings;

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