using UnityEngine;
using System.Linq;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class ChunkRenderer : MonoBehaviour
    {
        [SerializeField] private MeshCollider _meshCollider;
        [SerializeField] private MeshFilter _meshFilter;
        [SerializeField] private Transform _visual;
        private Mesh _mesh;
        private Mesh _colliderMesh;

        public ChunkData ChunkData { get; private set; }

        private void Awake() 
        { 
            _mesh = _meshFilter.mesh;
            _colliderMesh = new Mesh();
        }

        public void InitializeChunk(ChunkData data) => ChunkData = data;

        public void UpdateChunk() => UpdateChunk(MeshDataBuilder.GenerateMeshData(ChunkData));
        
        public void UpdateChunk(MeshData meshData)
        {
            RenderMesh(meshData);
            SetCollider(meshData);
        }

        private void RenderMesh(MeshData meshData)
        {
            _mesh.Clear();
            _mesh.subMeshCount = 2;

            _mesh.SetVertices(meshData.Vertices.Concat(meshData.WaterMeshData.Vertices).ToArray());
            _mesh.SetTriangles(meshData.WaterMeshData.Triangles.Select(val => val + meshData.Vertices.Count).ToArray(), 1);

            _mesh.SetTriangles(meshData.Triangles, 0);
            _mesh.SetUVs(0, meshData.UVs.Concat(meshData.WaterMeshData.UVs).ToArray());

            _mesh.RecalculateNormals();
        }

        private void SetCollider(MeshData meshData)
        {
            _colliderMesh.Clear();

            _colliderMesh.SetVertices(meshData.ColliderVerticesTriangles.Keys.ToArray());
            _colliderMesh.SetTriangles(meshData.ColliderTriangles, 0);

            _meshCollider.sharedMesh = _colliderMesh;
        }
    }
}