using UnityEngine;
using System;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    [CreateAssetMenu(fileName = "Voxel", menuName = "World Generation/Voxel")]
    public class VoxelData : ScriptableObject
    {
        public VoxelType type;
        public TextureData textureData;
        public bool generatesCollider;
    }

    [Serializable]
    public struct TextureData
    {
        public Vector2Int up;
        public Vector2Int down;
        public Vector2Int side;
    }
}