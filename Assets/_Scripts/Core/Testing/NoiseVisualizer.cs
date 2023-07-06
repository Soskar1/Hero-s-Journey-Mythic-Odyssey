using System.Collections.Generic;
using HerosJourney.Core.WorldGeneration.Noises;
using HerosJourney.Utils;
using UnityEngine;

namespace HerosJourney.Core.Testing
{
    public class NoiseVisualizer : MonoBehaviour
    {
        [SerializeField] private NoiseSettings _noiseSettings;
        [SerializeField] private StructureGenerationSettings _generationSettings;
        [SerializeField] private Renderer _renderer;

        [SerializeField] private int seed;
        [SerializeField] private int size;
        [SerializeField] private Vector2Int _offset;

        [SerializeField] private bool _showLocalMaximas;
        [SerializeField] private bool _showPointsAboveThreshold;
        [SerializeField] private float _threshold;
        [SerializeField] private Color _localMaximaColor;
        [SerializeField] private Color _highPointColor;

        private float[,] _currentNoiseData;

        private void Awake() => _currentNoiseData = new float[size, size];

        public void VisualizeNoise()
        {
            Noise.SetSeed(seed);

            _currentNoiseData = Noise.GenerateNoise(size, _offset, _noiseSettings);
            Texture2D generatedTexture = GenerateTexture();

            if (_showLocalMaximas)
                AddLocalMaximas(generatedTexture);

            if (_showPointsAboveThreshold)
                ShowPointsAboveThreshold(generatedTexture);

            _renderer.material.mainTexture = generatedTexture;
        }

        private Texture2D GenerateTexture()
        {
            Texture2D texture = new Texture2D(size, size);

            for (int x = 0; x < size; ++x)
            {
                for (int y = 0; y < size; ++y)
                {
                    Color color = new Color(_currentNoiseData[x, y], _currentNoiseData[x, y], _currentNoiseData[x, y]);
                    texture.SetPixel(x, y, color); 
                }
            }

            texture.Apply();
            return texture;
        }

        private void AddLocalMaximas(Texture2D texture)
        {
            List<Vector2Int> localMaximas = Noise.FindLocalMaximas(_currentNoiseData);

            foreach (var maxima in localMaximas)
                texture.SetPixel(maxima.x, maxima.y, _localMaximaColor);

            texture.Apply();
        }

        private void ShowPointsAboveThreshold(Texture2D texture)
        {
            List<Vector2Int> points = Noise.FindPointsAboveThreshold(_currentNoiseData, _threshold, 
                () => ThreadSafeRandom.NextDouble() <= _generationSettings.probability);

            foreach (var point in points)
                texture.SetPixel(point.x, point.y, _highPointColor);

            texture.Apply();
        }
    }
}