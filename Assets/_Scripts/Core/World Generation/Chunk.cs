using System.Linq;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField] private byte _chunkLength;
        [SerializeField] private byte _chunkHeight;
        [SerializeField] private MeshFilter _meshFilter;
        private Mesh _mesh;

        private void Awake() => _mesh = _meshFilter.mesh;

        private void Start()
        {
            var position = transform.position;

            var blocks = new NativeArray<VoxelType>(32768, Allocator.TempJob);

            for (int x = 0; x < _chunkLength; ++x)
            {
                for (int z = 0; z < _chunkLength; ++z)
                {
                    var y = Mathf.FloorToInt(Mathf.PerlinNoise((position.x + x) * 0.15f, (position.z + z) * 0.15f) * _chunkHeight);

                    for (int i = 0; i < y; ++i)
                        blocks[VoxelExtensions.GetVoxelIndex(new int3(x, i, z))] = VoxelType.Stone;

                    for (int i = y; i < _chunkHeight; ++i)
                        blocks[VoxelExtensions.GetVoxelIndex(new int3(x, i, z))] = VoxelType.Air;
                }
            }

            var meshData = new ChunkJob.MeshData {
                vertices = new NativeList<int3>(Allocator.TempJob),
                triangles = new NativeList<int>(Allocator.TempJob)
            };

            var chunkData = new ChunkJob.ChunkData {
                voxels = blocks,
                height = _chunkHeight,
                length = _chunkLength
            };

            var blockData = new ChunkJob.VoxelGeometry {
                vertices = VoxelGeometry.vertices,
                triangles = VoxelGeometry.triangles
            };

            var job = new ChunkJob {
                meshData = meshData,
                chunkData = chunkData,
                voxelGeometry = blockData
            };

            JobHandle jobHandle = job.Schedule();
            jobHandle.Complete();

            _mesh.vertices = meshData.vertices.ToArray().Select(vertex => new Vector3(vertex.x, vertex.y, vertex.z)).ToArray();
            _mesh.SetIndices(meshData.triangles.AsArray(), MeshTopology.Triangles, 0);

            meshData.vertices.Dispose();
            meshData.triangles.Dispose();
            blocks.Dispose();

            _mesh.RecalculateNormals();
            _mesh.RecalculateBounds();
            _mesh.RecalculateTangents();
        }
    }
}