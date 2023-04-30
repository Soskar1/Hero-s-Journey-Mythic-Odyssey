using UnityEngine;
using System.Linq;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class ChunkRenderer : MonoBehaviour
    {
        [SerializeField] private MeshCollider _meshCollider;
        [SerializeField] private MeshFilter _meshFilter;
        private Mesh _mesh;

        public ChunkData ChunkData { get; private set; }

        private void Awake() => _mesh = _meshFilter.mesh;

        public void InitializeChunk(ChunkData data) => ChunkData = data;

        public void UpdateChunk() => RenderMesh(ChunkDataHandler.GenerateMeshData(ChunkData));
        public void UpdateChunk(MeshData data) => RenderMesh(data);

        private void RenderMesh(MeshData meshData)
        {
            _mesh.Clear();

            _mesh.subMeshCount = 2;
            _mesh.SetVertices(meshData.Vertices.Concat(meshData.WaterMeshData.Vertices).ToArray());
            _mesh.SetTriangles(meshData.Triangles, 0);
            _mesh.SetTriangles(meshData.WaterMeshData.Triangles.Select(val => val + meshData.Vertices.Count).ToArray(), 1);
            _mesh.SetUVs(0, meshData.UVs.Concat(meshData.WaterMeshData.UVs).ToArray());

            Mesh colliderMesh = new Mesh();
            colliderMesh.SetVertices(meshData.ColliderVertices);
            colliderMesh.SetTriangles(meshData.ColliderTriangles, 0);

            _meshCollider.sharedMesh = colliderMesh;

            _mesh.RecalculateNormals();
        }
    }
}