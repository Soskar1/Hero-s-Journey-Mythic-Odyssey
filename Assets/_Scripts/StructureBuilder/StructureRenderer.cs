using UnityEngine;
using Zenject;

namespace HerosJourney.StructureBuilder
{
    public class StructureRenderer : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        private Mesh _mesh;

        private StructureData _structureData;

        private void Awake() => _mesh = _meshFilter.mesh;

        [Inject]
        private void Construct(StructureData data) => _structureData = data;

        [ContextMenu("UpdateMesh")]
        public void UpdateStructure() => UpdateStructure(_structureData.mesh);

        public void UpdateStructure(StructureMesh meshData) => RenderMesh(meshData);

        private void RenderMesh(StructureMesh meshData)
        {
            _mesh.Clear();

            _mesh.SetVertices(meshData.Vertices.ToArray());

            _mesh.SetTriangles(meshData.Triangles, 0);
            _mesh.SetUVs(0, meshData.UVs.ToArray());

            _mesh.RecalculateNormals();
        }
    }
}