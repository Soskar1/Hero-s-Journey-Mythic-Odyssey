using UnityEngine;

namespace HerosJourney.Utils
{
    public enum Direction
    {
        up,
        down,
        right,
        left,
        forward,
        back
    }

    public static class DirectionExtensions
    {
        public static Vector3Int ToVector3Int(this Direction direction)
        {
            return direction switch
            {
                Direction.up => Vector3Int.up,
                Direction.down => Vector3Int.down,
                Direction.forward => Vector3Int.forward,
                Direction.right => Vector3Int.right,
                Direction.left => Vector3Int.left,
                Direction.back => Vector3Int.back,
                _ => throw new System.Exception("Invalid input direction")
            };
        }
    }
}