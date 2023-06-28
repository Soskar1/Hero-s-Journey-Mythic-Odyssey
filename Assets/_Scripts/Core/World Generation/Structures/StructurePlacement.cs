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

        public static List<Vector2Int> PlaceStructures(float[,] noise, Vector2Int worldPosition, StructurePlacementSettings settings)
        {
            List<Vector2Int> structurePositions = new List<Vector2Int>();

            for (int x = 0; x < noise.GetLength(0); x += settings.radius)
            {
                for (int y = 0; y < noise.GetLength(1); y += settings.radius)
                {
                    Vector2Int localPosition = new Vector2Int(x, y);
                    Vector2Int localMaxima = FindLocalMaxima(noise, localPosition, settings);

                    if (localMaxima != localPosition || (localMaxima == localPosition && noise[x,y] < settings.threshold))
                        structurePositions.Add(localMaxima + worldPosition);
                }
            }
                
            return structurePositions;
        }

        private static Vector2Int FindLocalMaxima(float[,] noise, Vector2Int localPosition, StructurePlacementSettings settings)
        {
            Vector2Int localMaxima = localPosition;
            float maxThreshold = settings.threshold;

            foreach (var dir in directions)
            {
                var newPos = new Vector2Int(dir.x + localPosition.x, dir.y + localPosition.y);

                if (newPos.x < 0 || newPos.x >= noise.GetLength(0) || newPos.y < 0 || newPos.y >= noise.GetLength(1))
                    continue;

                if (noise[newPos.x, newPos.y] < maxThreshold)
                    continue;

                maxThreshold = noise[newPos.x, newPos.y];
                localMaxima = newPos;
            }

            return localMaxima;
        }
    }
}