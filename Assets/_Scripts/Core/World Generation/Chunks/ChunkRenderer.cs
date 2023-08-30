using System.Linq;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class ChunkRenderer : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        private Mesh _mesh;

        private void Awake() => _mesh = _meshFilter.mesh;

        public void Render(MeshData meshData)
        {
            _mesh.vertices = meshData.Vertices.Select(vertex => new Vector3(vertex.x, vertex.y, vertex.z)).ToArray();
            _mesh.SetIndices(meshData.Triangles, MeshTopology.Triangles, 0);
            _mesh.SetUVs(0, meshData.UVs.Select(uv => new Vector2(uv.x, uv.y)).ToArray());

            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            _mesh.RecalculateTangents();
        }
    }
}