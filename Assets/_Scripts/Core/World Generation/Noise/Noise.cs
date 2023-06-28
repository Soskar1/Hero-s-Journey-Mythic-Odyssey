using System;
using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Noises
{
    public static class Noise
    {
        private static List<Vector2Int> directions = new List<Vector2Int>
        {
            new Vector2Int(0, 1),
            new Vector2Int(1, 1),
            new Vector2Int(1, 0),
            new Vector2Int(1, -1),
            new Vector2Int(0, -1),
            new Vector2Int(-1, -1),
            new Vector2Int(-1, 0),
            new Vector2Int(-1, 1)
        };

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
        
        public static float[,] GenerateNoise(int size, Vector2Int worldPosition, NoiseSettings noiseSettings)
        {
            float[,] noise = new float[size, size];

            int xStart = worldPosition.x;
            int xEnd = worldPosition.x + size;
            int yStart = worldPosition.y;
            int yEnd = worldPosition.y + size;

            int xIndex = 0;
            int yIndex = 0;
            for (int x = xStart; x < xEnd; ++x)
            {
                for (int y = yStart; y < yEnd; ++y)
                {
                    noise[xIndex, yIndex] = Noise.OctavePerlinNoise(x, y, noiseSettings);

                    ++yIndex;
                }
                ++xIndex;
                yIndex = 0;
            }

            return noise;
        }

        public static List<Vector2Int> FindLocalMaximas(float[,] noise, Vector2Int worldPosition)
        {
            List<Vector2Int> localMaximas = new List<Vector2Int>();

            for (int x = 0; x < noise.GetLength(0); ++x)
            {
                for (int y = 0; y < noise.GetLength(1); ++y)
                {
                    Vector2Int localPosition = new Vector2Int(x, y);
                    if (CheckNeighbours(noise, localPosition, (neighbourNoise) => neighbourNoise < noise[x, y]))
                        localMaximas.Add(localPosition + worldPosition);
                }
            }
                
            return localMaximas;
        }

        private static bool CheckNeighbours(float[,] noise, Vector2Int localPosition, Func<float, bool> condition)
        {
            foreach (var dir in directions)
            {
                var newPos = new Vector2Int(dir.x + localPosition.x, dir.y + localPosition.y);

                if (newPos.x < 0 || newPos.x >= noise.GetLength(0) || newPos.y < 0 || newPos.y >= noise.GetLength(1))
                    continue;

                if (condition(noise[newPos.x, newPos.y]) == false)
                    return false;
            }

            return true;
        }
    }
}