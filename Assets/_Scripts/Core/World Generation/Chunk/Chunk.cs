using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class Chunk : MonoBehaviour
    {
        public ChunkData ChunkData { get; private set; }

        public void InitializeChunk(ChunkData data) => ChunkData = data;

        public void SetVoxel(VoxelData voxelData, Vector3Int localPosition)
        {
            if (InBounds(localPosition)) 
                ChunkData.voxels[localPosition.x, localPosition.y, localPosition.z].data = voxelData;
        }

        public bool InBounds(Vector3Int localPosition)
        {
            if (localPosition.x < 0 || localPosition.x > ChunkData.ChunkSize ||
                localPosition.y < 0 || localPosition.y > ChunkData.ChunkHeight ||
                localPosition.z < 0 || localPosition.z > ChunkData.ChunkSize)
                return false;

            return true;
        }
    }
}