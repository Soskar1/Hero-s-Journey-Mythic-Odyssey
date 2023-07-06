using System;
using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class MeshData
    {
        public List<Vector3> Vertices { get; private set; }
        public List<int> Triangles { get; private set; }
        public List<Vector3> UVs { get; private set; }

        public Dictionary<Vector3, int> ColliderVerticesTriangles { get; private set; }
        public List<int> ColliderTriangles { get; private set; }

        public MeshData WaterMeshData { get; private set; }

        public MeshData(bool isMainMesh)
        {
            Vertices = new List<Vector3>();
            Triangles = new List<int>();
            UVs = new List<Vector3>();

            ColliderVerticesTriangles = new Dictionary<Vector3, int>();
            ColliderTriangles = new List<int>();

            if (isMainMesh)
                WaterMeshData = new MeshData(false);
        }

        public void AddVertices(Vector3[] positions, bool generatesCollider)
        {
            foreach (var vertex in positions) 
            {
                Vertices.Add(vertex);

                if (generatesCollider)
                    ColliderVerticesTriangles.TryAdd(vertex, ColliderVerticesTriangles.Count);
            }
        }

        public void CreateQuad(Vector3[] vertices, bool generatesCollider)
        {
            if (vertices.Length != 4)
                throw new Exception("Vertices count must be equal to 4");

            Triangles.Add(Vertices.Count - 4);
            Triangles.Add(Vertices.Count - 3);
            Triangles.Add(Vertices.Count - 2);

            Triangles.Add(Vertices.Count - 4);
            Triangles.Add(Vertices.Count - 2);
            Triangles.Add(Vertices.Count - 1);

            if (generatesCollider)
            {
                ColliderTriangles.Add(ColliderVerticesTriangles[vertices[0]]);
                ColliderTriangles.Add(ColliderVerticesTriangles[vertices[1]]);
                ColliderTriangles.Add(ColliderVerticesTriangles[vertices[2]]);

                ColliderTriangles.Add(ColliderVerticesTriangles[vertices[0]]);
                ColliderTriangles.Add(ColliderVerticesTriangles[vertices[2]]);
                ColliderTriangles.Add(ColliderVerticesTriangles[vertices[3]]);
            }
        }

        public void AddUVCoordinates(Vector2[] uvCoordinates) 
        {
            foreach (var uv in uvCoordinates)
                UVs.Add(uv);
        }
    }
}