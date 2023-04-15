using HerosJourney.Core.Databases;
using System;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    [CreateAssetMenu(fileName = "Voxel", menuName = "World Generation/Voxel")]
    public class VoxelData : ScriptableObject, IIdentifiable
    {
        public VoxelType type;
        public TextureData textureData;
        public int blockID;

        public int Id => blockID;
    }

    [Serializable]
    public struct TextureData
    {
        public Vector2Int up;
        public Vector2Int down;
        public Vector2Int side;
    }
}