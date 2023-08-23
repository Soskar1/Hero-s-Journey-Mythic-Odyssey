using Unity.Mathematics;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class Chunk
    {
        public ChunkRenderer renderer;
        private ChunkData _data;
        private MeshData _meshData;
        private int3 _worldPosition;
        
        public Transform Transform
        {
            get
            {
                if (renderer is null)
                {
                    Debug.LogError("Cannot return Transform! ChunkRenderer is null!");
                    return null;
                }

                return renderer.transform;
            }
        }
        public ChunkData Data => _data;
        public MeshData MeshData => _meshData;
        public int3 WorldPosition => _worldPosition;
        public ushort[] Voxels => _data.voxels;

        public Chunk(ChunkData data, MeshData meshData, int3 worldPosition)
        {
            _data = data;
            _meshData = meshData;
            _worldPosition = worldPosition;
        }
        
        public void Render() => renderer.RenderMesh(_meshData);

        public void Dispose() => _meshData.Dispose();
    }
}