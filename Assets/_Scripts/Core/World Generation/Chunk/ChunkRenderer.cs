using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class ChunkRenderer : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        private Mesh _mesh;

        private void Awake() => _mesh = _meshFilter.mesh;

        public void UpdateChunk(MeshData data) => RenderMesh(data);

        private void RenderMesh(MeshData meshData)
        {
            _mesh.Clear();

            _mesh.SetVertices(meshData.Vertices);
            _mesh.SetTriangles(meshData.Triangles, 0);

            _mesh.RecalculateNormals();
        }
    }
}