using System;

namespace HerosJourney.Core.WorldGeneration.Structures
{
    [Serializable]
    public struct StructurePlacementSettings
    {
        public float radius;
        public float threshold;

        public StructurePlacementSettings(float radius, float threshold)
        {
            this.radius = radius;
            this.threshold = threshold;
        }
    }
}