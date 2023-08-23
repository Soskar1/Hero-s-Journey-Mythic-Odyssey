using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class ChunkRenderer : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        private Mesh _mesh;

        private void Awake() => _mesh = _meshFilter.mesh;

        public void RenderMesh(MeshData meshData)
        {
            _mesh.Clear();
            _mesh.subMeshCount = 2;
            
            _mesh.SetVertices(meshData.vertices.AsArray());
            _mesh.SetIndices(meshData.triangles.AsArray(), MeshTopology.Triangles, 0);
            _mesh.SetUVs(0, meshData.uvs.AsArray());

            _mesh.RecalculateNormals();

            Debug.Log("Triangles.Length: " + _mesh.triangles.Length);
            Debug.Log("Vertices.Length: " + _mesh.vertices.Length);
        }
    }
}