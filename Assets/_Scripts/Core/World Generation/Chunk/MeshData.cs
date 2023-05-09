using System.Collections.Generic;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class MeshData
    {
        public List<Vector3> Vertices { get; private set; }
        public List<int> Triangles { get; private set; }
        public List<Vector3> Normals { get; private set; }
        public List<Vector3> UVs { get; private set; }

        public List<Vector3> ColliderVertices { get; private set; }
        public List<int> ColliderTriangles { get; private set; }

        public MeshData WaterMeshData { get; private set; }

        public MeshData(bool isMainMesh) {
            Vertices = new List<Vector3>();
            Triangles = new List<int>();
            Normals = new List<Vector3>();
            UVs = new List<Vector3>();

            ColliderVertices = new List<Vector3>();
            ColliderTriangles = new List<int>();

            if (isMainMesh)
                WaterMeshData = new MeshData(false);
        }

        public void AddVertex(Vector3 position) => Vertices.Add(position);

        public void AddNormal(Vector3 normal) => Normals.Add(normal);

        public void CreateQuad()
        {
            Triangles.Add(Vertices.Count - 4);
            Triangles.Add(Vertices.Count - 3);
            Triangles.Add(Vertices.Count - 2);

            Triangles.Add(Vertices.Count - 4);
            Triangles.Add(Vertices.Count - 2);
            Triangles.Add(Vertices.Count - 1);
        }

        public void AddUVCoordinates(Vector3 uvCoordinates) => UVs.Add(uvCoordinates);
    }
}