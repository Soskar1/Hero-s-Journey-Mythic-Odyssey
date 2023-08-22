using System.Linq;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class ChunkRenderer : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        private Mesh _mesh;

        private void Awake() => _mesh = _meshFilter.mesh;

        public void UpdateChunk(MeshData meshData)
        {
            RenderMesh(meshData);
        }

        private void RenderMesh(MeshData meshData)
        {
            _mesh.Clear();
            _mesh.subMeshCount = 2;

            _mesh.SetVertices(meshData.vertices);
            _mesh.SetTriangles(meshData.WaterMeshData.Triangles.Select(val => val + meshData.Vertices.Count).ToArray(), 1);

            _mesh.SetTriangles(meshData.Triangles, 0);
            _mesh.SetUVs(0, meshData.UVs.Concat(meshData.WaterMeshData.UVs).ToArray());

            _mesh.RecalculateNormals();
        }
    }
}