using HerosJourney.Core.WorldGeneration.Chunks;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Biomes
{
    public class BiomeGenerator : MonoBehaviour
    {
        [SerializeField] private LayerGenerator _startingLayer;

        public ChunkData GenerateChunk(ChunkData data, int x, int z, Vector2Int offset)
        {


            return data;
        }
    }
}