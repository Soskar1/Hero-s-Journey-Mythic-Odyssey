using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Structures
{
    public static class StructurePlacement
    {
        public static List<Vector2Int> PlaceStructures(float[,] noise, int worldX, int worldZ, float threshold)
        {
            List<Vector2Int> structurePositions = new List<Vector2Int>();

            for (int x = 0; x < noise.GetLength(0); ++x)
                for (int z = 0; z < noise.GetLength(1); ++z)
                    if (noise[x, z] > threshold)
                        structurePositions.Add(new Vector2Int(worldX + x, worldZ + z));

            return structurePositions;
        }
    }
}