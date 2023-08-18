using System;
using Unity.Mathematics;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    public class VoxelData : ScriptableObject
    {
        public int id;
        public VoxelType type;
        public TextureData textureData;
        public bool generatesCollider;
    }

    public struct TSVoxelData
    {
        public int id;
        public VoxelType type;
        public TextureData textureData;
        public bool generatesCollider;

        public TSVoxelData(VoxelData voxelData)
        {
            id = voxelData.id;
            type = voxelData.type;
            textureData = voxelData.textureData;
            generatesCollider = voxelData.generatesCollider;
        }
    }

    [Serializable]
    public struct TextureData
    {
        public int2 up;
        public int2 down;
        public int2 side;
    }
}