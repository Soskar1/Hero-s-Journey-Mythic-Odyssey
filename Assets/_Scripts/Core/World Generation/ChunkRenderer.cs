using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class ChunkRenderer : MonoBehaviour
    {
        [SerializeField] private MeshFilter _meshFilter;
        private Mesh _mesh;

        private void Awake() => _mesh = _meshFilter.mesh;
    }
}