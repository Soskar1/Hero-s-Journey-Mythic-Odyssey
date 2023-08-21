using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public static class MeshDataBuilder
    {
        public static MeshData GenerateMeshData(ChunkData chunkData)
        {
            MeshData meshData = new MeshData();

            //for (int x = 0; x < chunkData.ChunkLength; ++x)
            //    for (int y = 0; y < chunkData.ChunkHeight; ++y)
            //        for (int z = 0; z < chunkData.ChunkLength; ++z)
            //            GenerateVoxelFaces(chunkData, meshData, chunkData.voxels[x, y, z].data, new Vector3Int(x, y, z));

            return meshData;
        }
    }
}