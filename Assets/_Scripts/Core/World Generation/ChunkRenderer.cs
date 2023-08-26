using System.Linq;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class ChunkRenderer : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        private Mesh _mesh;

        private void Awake() => _mesh = _meshFilter.mesh;

        public void Render(MeshData meshData)
        {
            _mesh.vertices = meshData.vertices.ToArray().Select(vertex => new Vector3(vertex.x, vertex.y, vertex.z)).ToArray();
            _mesh.SetIndices(meshData.triangles.AsArray(), MeshTopology.Triangles, 0);

            meshData.vertices.Dispose();
            meshData.triangles.Dispose();

            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            _mesh.RecalculateTangents();
        }
    }
}