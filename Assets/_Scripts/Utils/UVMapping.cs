using UnityEngine;
using System;

namespace HerosJourney.Utils
{
    public class UVMapping : MonoBehaviour
    {
        [SerializeField] private int _tileSize = 16;
        [SerializeField] private Texture2D _atlas;

        private static float _offset = 0.001f;
        private static float _xStep;
        private static float _yStep;

        public static float AtlasWidth { get; private set; }
        public static float AtlasHeight { get; private set; }
        public static int TileSize { get; private set; }

        private void Awake()
        {
            _xStep = _tileSize / (float)_atlas.width;
            _yStep = _tileSize / (float)_atlas.height;

            AtlasWidth = _atlas.width;
            AtlasHeight = _atlas.height;
            TileSize = _tileSize;
        }

        public static Vector2[] GetUVCoordinates(Vector2Int textureCoordinates)
        {
            if (textureCoordinates.x > AtlasWidth / TileSize ||
                textureCoordinates.y > AtlasHeight / TileSize)
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