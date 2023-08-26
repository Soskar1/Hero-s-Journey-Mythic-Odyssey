using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;

namespace HerosJourney.Core.WorldGeneration.Chunks
{
    public class MeshDataBuilder : IDisposable
    {
        private NativeList<JobHandle> _scheduledJobs;
        private Dictionary<int3, MeshData> _generatedMeshData = new Dictionary<int3, MeshData>();
        private List<IDisposable> _toDispose = new List<IDisposable>();

        public MeshDataBuilder() => _scheduledJobs = new NativeList<JobHandle>(Allocator.Persistent);
        public void Dispose() => _scheduledJobs.Dispose();

        public void ScheduleMeshGenerationJob(List<ChunkData> generatedChunkData)
        {
            _generatedMeshData.Clear();

            foreach (ChunkData chunkData in generatedChunkData)
            {
                MeshData meshData = new MeshData
                {
                    vertices = new NativeList<int3>(Allocator.TempJob),
                    triangles = new NativeList<int>(Allocator.TempJob),
                };

                var TSchunkData = new TSChunkData(chunkData);

                var voxelGeometry = new MeshGenerationJob.VoxelGeometry
                {
                    vertices = VoxelGeometry.vertices,
                    triangles = VoxelGeometry.triangles,
                };

                MeshGenerationJob job = new MeshGenerationJob
                {
                    meshData = meshData,
                    voxelGeometry = voxelGeometry,
                    chunkData = TSchunkData
                };

                JobHandle jobHandle = job.Schedule();
                _generatedMeshData.Add(chunkData.WorldPosition, meshData);
                _scheduledJobs.Add(jobHandle);
                _toDispose.Add(TSchunkData);
            }
        }

        public Dictionary<int3, MeshData> Complete()
        {
            JobHandle.CompleteAll(_scheduledJobs.AsArray());

            foreach (var data in _toDispose)
                data.Dispose();

            _toDispose.Clear();
            _scheduledJobs.Clear();

            return _generatedMeshData;
        }
    }
}