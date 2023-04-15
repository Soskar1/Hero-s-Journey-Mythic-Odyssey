using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class ChunkRenderer : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        private Mesh _mesh;

        public ChunkData ChunkData { get; private set; }

        private void Awake() => _mesh = _meshFilter.mesh;

        public void InitializeChunk(ChunkData data) => ChunkData = data;

        public void UpdateChunk() => RenderMesh(ChunkVoxelData.GetChunkMeshData(ChunkData));
        public void UpdateChunk(MeshData data) => RenderMesh(data);

        private void RenderMesh(MeshData meshData)
        {
            _mesh.Clear();

            _mesh.subMeshCount = 2;
            _mesh.SetVertices(meshData.Vertices);
            _mesh.SetTriangles(meshData.Triangles, 0);
            _mesh.SetUVs(0, meshData.UVs);

            GetComponent<MeshCollider>().sharedMesh = _mesh;

            _mesh.RecalculateNormals();
        }
    }
}