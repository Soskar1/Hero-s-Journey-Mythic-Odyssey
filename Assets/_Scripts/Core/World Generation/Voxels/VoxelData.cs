using System;
using Unity.Mathematics;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Voxels
{
    [CreateAssetMenu(fileName = "new VoxelData", menuName = "World Generation/VoxelData")]
    public class VoxelData : ScriptableObject
    {
        public ushort id;
        public VoxelType type;
        public TextureData textureData;
        public bool generatesCollider;
    }

    public struct TSVoxelData
    {
        public ushort id;
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

        public static implicit operator TSVoxelData(VoxelData voxelData) => new TSVoxelData(voxelData);
    }

    [Serializable]
    public struct TextureData
    {
        public int2 up;
        public int2 down;
        public int2 side;
    }
}