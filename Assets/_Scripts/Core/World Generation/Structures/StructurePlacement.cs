using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Structures
{
    public static class StructurePlacement
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

        public static List<Vector2Int> PlaceStructures(float[,] noise, int worldX, int worldZ, StructurePlacementSettings settings)
        {
            List<Vector2Int> structurePositions = new List<Vector2Int>();

            for (int x = 0; x < noise.GetLength(0); ++x)
                for (int z = 0; z < noise.GetLength(1); ++z)
                    if (noise[x, z] > settings.threshold)
                        structurePositions.Add(new Vector2Int(worldX + x, worldZ + z));

            return structurePositions;
        }

        private static Vector2Int GetLocalMaximum(float[,] noise, int worldX, int worldZ, StructurePlacementSettings settings)
        {
            Vector2Int
        }
    }
}