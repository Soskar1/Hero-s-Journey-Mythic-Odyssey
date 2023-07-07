using UnityEngine;
using System;

namespace HerosJourney.Utils
{
    public static class UVMapping
    {
        private static int _tileSize = 16;
        private static Texture2D _atlas;

        private static int _width;
        private static int _height;

        private static float _offset = 0.001f;
        private static float _xStep;
        private static float _yStep;

        public static void Initialize(Texture2D atlas, int tileSize)
        {
            _tileSize = tileSize;
            _atlas = atlas;

            _xStep = _tileSize / (float)_atlas.width;
            _yStep = _tileSize / (float)_atlas.height;

            _width = _atlas.width;
            _height = _atlas.height;
        }

        public static Vector2[] GetUVCoordinates(Vector2Int textureCoordinates)
        {
            if (textureCoordinates.x > _width / _tileSize ||
                textureCoordinates.y > _height / _tileSize)
                throw new ArgumentOutOfRangeException();

            Vector2[] uvCoordinates = new Vector2[4];

            uvCoordinates[0] = new Vector2(_xStep * textureCoordinates.x + _offset, _yStep * textureCoordinates.y + _offset);
            uvCoordinates[1] = new Vector2(_xStep * textureCoordinates.x + _offset, _yStep * textureCoordinates.y + _yStep - _offset);
            uvCoordinates[2] = new Vector2(_xStep * textureCoordinates.x + _xStep - _offset, _yStep * textureCoordinates.y + _yStep - _offset);
            uvCoordinates[3] = new Vector2(_xStep * textureCoordinates.x + _xStep - _offset, _yStep * textureCoordinates.y + _offset);

            return uvCoordinates;
        }
    }
}