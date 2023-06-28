using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Noises
{
    public class NoiseGenerator : MonoBehaviour
    {
        [SerializeField] private NoiseSettings _noiseSettings;

        public float[,] GenerateNoise(int size, Vector2Int worldPosition)
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
                    noise[xIndex, yIndex] = Noise.OctavePerlinNoise(x, y, _noiseSettings);

                    ++yIndex;
                }
                ++xIndex;
                yIndex = 0;
            }

            return noise;
        }
    }
}