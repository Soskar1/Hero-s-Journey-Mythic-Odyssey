using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.StructureBuilder
{
    public class StructureMesh
    {
        public List<Vector3> Vertices { get; private set; }
        public List<int> Triangles { get; private set; }
        public List<Vector3> UVs { get; private set; }

        public StructureMesh()
        {
            Vertices = new List<Vector3>();
            Triangles = new List<int>();
            UVs = new List<Vector3>();
        }

        public void AddVertices(Vector3[] positions)
        {
            foreach (var vertex in positions) 
                Vertices.Add(vertex);
        }

        public void CreateQuad(Vector3[] vertices)
        {
            if (vertices.Length != 4)
                throw new System.Exception("Vertices count must be equal to 4");

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
