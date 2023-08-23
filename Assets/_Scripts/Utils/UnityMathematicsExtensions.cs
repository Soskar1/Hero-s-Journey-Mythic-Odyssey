using Unity.Mathematics;
using UnityEngine;

namespace HerosJourney.Utils
{
    public static class UnityMathematicsExtensions
    {
        public static Vector3 ToVector3(this int3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }

        public static Vector3 ToVector3(this float3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }
    }
}