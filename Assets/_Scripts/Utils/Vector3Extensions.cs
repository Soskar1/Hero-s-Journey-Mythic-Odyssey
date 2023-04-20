using UnityEngine;

namespace HerosJourney.Utils
{
    public static class Vector3Extensions
    {
        public static Vector3Int ToVector3Int (this Vector3 vector3) {
            return new Vector3Int
            {
                x = (int)vector3.x,
                y = (int)vector3.y,
                z = (int)vector3.z
            };
        }
    }
}
