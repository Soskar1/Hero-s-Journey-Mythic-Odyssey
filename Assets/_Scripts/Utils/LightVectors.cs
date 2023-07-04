using UnityEngine;
using System;

namespace HerosJourney.Utils
{
    [Serializable]
    public struct LightVector2Int
    {
        public int x;
        public int y;

        public LightVector2Int(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static implicit operator Vector2Int(LightVector2Int v) => new Vector2Int(v.x, v.y);
    }

    [Serializable]
    public struct LightVector3Int
    {
        public int x;
        public int y;
        public int z;

        public LightVector3Int(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator Vector3Int(LightVector3Int v) => new Vector3Int(v.x, v.y, v.z);
    }
}