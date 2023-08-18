using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class MeshData
    {
        public List<Vector3> Vertices { get; private set; }
        public List<int> Triangles { get; private set; }
        public List<Vector3> UVs { get; private set; }

        public MeshData()
        {
            Vertices = new List<Vector3>();
            Triangles = new List<int>();
            UVs = new List<Vector3>();
        }

        public void AddVertices(Vector3[] positions, bool generatesCollider)
        {
            foreach (var vertex in positions)
                Vertices.Add(vertex);
        }

        public void CreateQuad(Vector3[] vertices, bool generatesCollider)
        {
            Triangles.Add(Vertices.Count - 4);
            Triangles.Add(Vertices.Count - 3);
            Triangles.Add(Vertices.Count - 2);

            Triangles.Add(Vertices.Count - 4);
            Triangles.Add(Vertices.Count - 2);
            Triangles.Add(Vertices.Count - 1);
        }

        public void AddUVCoordinates(Vector2[] uvCoordinates)
        {
            foreach (var uv in uvCoordinates)
                UVs.Add(uv);
        }
    }
}