using System;

namespace HerosJourney.Core.WorldGeneration.Structures
{
    [Serializable]
    public struct StructurePlacementSettings
    {
        public int radius;
        public float threshold;

        public StructurePlacementSettings(int radius, float threshold)
        {
            this.radius = radius;
            this.threshold = threshold;
        }
    }
}