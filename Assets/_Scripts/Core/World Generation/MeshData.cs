using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class MeshData
    {
        private List<Vector3> _vertices = new List<Vector3>();
        private List<int> _triangles = new List<int>();

        public List<Vector3> Vertices => _vertices;
        public List<int> Triangles => _triangles;

        public void AddVertex(Vector3 position) => _vertices.Add(position);

        public void CreateQuad()
        {
            _triangles.Add(_vertices.Count - 4);
            _triangles.Add(_vertices.Count - 3);
            _triangles.Add(_vertices.Count - 2);

            _triangles.Add(_vertices.Count - 4);
            _triangles.Add(_vertices.Count - 2);
            _triangles.Add(_vertices.Count - 1);
        }
    }
}