using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Noises
{
    public static class Noise
    {
        public static float OctavePerlinNoise(float x, float y, NoiseSettings noiseSettings)
        {
            x *= noiseSettings.noiseZoom;
            y *= noiseSettings.noiseZoom;

            float total = 0;
            float maxValue = 0;

            float amplitude = 1;
            float frequency = 1;

            for (int i = 0; i < noiseSettings.octaves; ++i)
            {
                //TODO: Add seed
                total += Mathf.PerlinNoise((x + noiseSettings.offset.x) * frequency, (y + noiseSettings.offset.y) * frequency) * amplitude;

                maxValue += amplitude;

                amplitude *= noiseSettings.persistence;
                frequency *= 2;
            }

            return total / maxValue;
        }
    }
}