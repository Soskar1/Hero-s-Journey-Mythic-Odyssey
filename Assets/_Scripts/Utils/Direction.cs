using Unity.Mathematics;

namespace HerosJourney.Utils
{
    public enum Direction
    {
        forward,
        right,
        back,
        left,
        up,
        down
    }

    public static class DirectionExtensions
    {
        public static int3 ToInt3(this Direction direction)
        {
            return direction switch
            {
                Direction.up => new int3(0, 1, 0),
                Direction.down => new int3(0, -1, 0),
                Direction.forward => new int3(0, 0, 1),
                Direction.right => new int3(1, 0, 0),
                Direction.left => new int3(-1, 0, 0),
                Direction.back => new int3(0, 0, -1),
                _ => throw new System.Exception("Invalid input direction")
            };
        }
    }
}