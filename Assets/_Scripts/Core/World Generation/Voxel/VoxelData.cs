using HerosJourney.Utils;
using UnityEngine;
using System;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    [CreateAssetMenu(fileName = "Voxel", menuName = "World Generation/Voxel")]
    public class VoxelData : ScriptableObject
    {
        public int id;
        public VoxelType type;
        public TextureData textureData;
        public bool generatesCollider;
    }

    [Serializable]
    public struct TextureData
    {
        public LightVector2Int up;
        public LightVector2Int down;
        public LightVector2Int side;
    }
}