using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class MeshData
    {
        public List<int> Triangles { get; private set; }
        public Dictionary<Vector3, int> VerticesTriangles { get; private set; }
        public List<Vector3> Normals { get; private set; }
        public List<Vector3> UVs { get; private set; }

        public List<Vector3> ColliderVertices { get; private set; }
        public List<int> ColliderTriangles { get; private set; }

        public MeshData WaterMeshData { get; private set; }

        public MeshData(bool isMainMesh) {
            Triangles = new List<int>();
            VerticesTriangles = new Dictionary<Vector3, int>();
            Normals = new List<Vector3>();
            UVs = new List<Vector3>();

            ColliderVertices = new List<Vector3>();
            ColliderTriangles = new List<int>();

            if (isMainMesh)
                WaterMeshData = new MeshData(false);
        }

        public void TryAddVertices(Vector3[] positions, bool generatesCollider)
        {
            for (int index = 0; index < positions.Length; ++index)
                if (VerticesTriangles.TryAdd(positions[index], VerticesTriangles.Count) && generatesCollider)
                    ColliderVertices.Add(positions[index]);
        }

        public void CreateQuad(Vector3[] vertices, bool generatedCollider)
        {
            if (vertices.Length != 4)
                throw new System.Exception("Vertices count must be equal to 4");

            Triangles.Add(VerticesTriangles[vertices[0]]);
            Triangles.Add(VerticesTriangles[vertices[1]]);
            Triangles.Add(VerticesTriangles[vertices[2]]);

            Triangles.Add(VerticesTriangles[vertices[0]]);
            Triangles.Add(VerticesTriangles[vertices[2]]);
            Triangles.Add(VerticesTriangles[vertices[3]]);

            if (generatedCollider)
            {
                ColliderTriangles.Add(VerticesTriangles[vertices[0]]);
                ColliderTriangles.Add(VerticesTriangles[vertices[1]]);
                ColliderTriangles.Add(VerticesTriangles[vertices[2]]);

                ColliderTriangles.Add(VerticesTriangles[vertices[0]]);
                ColliderTriangles.Add(VerticesTriangles[vertices[2]]);
                ColliderTriangles.Add(VerticesTriangles[vertices[3]]);
            }
        }

        public void AddUVCoordinates(Vector3 uvCoordinates) => UVs.Add(uvCoordinates);
    }
}