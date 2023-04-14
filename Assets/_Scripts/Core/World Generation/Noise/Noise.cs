using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Noises
{
    public static class Noise
    {
        public static float OctavePerlinNoise(float x, float y, NoiseSettings noiseSettings)
        {
            float total = 0;
            float maxValue = 0;

            float currentAmplitude = noiseSettings.amplitude;
            float currentFrequency = noiseSettings.frequency;

            for (int i = 0; i < noiseSettings.octaves; ++i)
            {
                //TODO: Add seed
                total += Mathf.PerlinNoise((x + noiseSettings.offset.x) * currentFrequency * noiseSettings.size, (y + noiseSettings.offset.y) * currentFrequency * noiseSettings.size) * currentAmplitude;

                maxValue += noiseSettings.amplitude;

                currentAmplitude *= noiseSettings.persistence;
                currentFrequency *= 2;
            }

            return total / maxValue;
        }
    }
}