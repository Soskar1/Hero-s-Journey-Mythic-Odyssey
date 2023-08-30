using HerosJourney.Core.WorldGeneration.Chunks;
using HerosJourney.Core.WorldGeneration.Noises;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using Zenject;

namespace HerosJourney.Core.WorldGeneration
{
    public class TerrainGenerator : IInitializable, IDisposable
    {
        private readonly WorldData _worldData;
        private readonly ThreadSafeNoiseSettings _noiseSettings;
        private readonly ushort _airID;
        private readonly ushort _dirtID;
        private readonly ushort _grassID;
        private List<ThreadSafeChunkData> _generatedChunkData = new List<ThreadSafeChunkData>();
        private NativeList<JobHandle> _scheduledJobs;

        public TerrainGenerator(WorldData worldData, NoiseSettings noiseSettings, ushort airID, ushort dirtID, ushort grassID)
        {
            _worldData = worldData;
            _noiseSettings = noiseSettings;
            _airID = airID;
            _dirtID = dirtID;
            _grassID = grassID;
        }

        public void Initialize() => _scheduledJobs = new NativeList<JobHandle>(Allocator.Persistent);
        public void Dispose() => _scheduledJobs.Dispose();

        public Task Generate(List<int3> chunkDataPositionsToCreate)
        {
            return Task.Run(() =>
            {
                Schedule(chunkDataPositionsToCreate);
                Complete();
            });
        }

        private void Schedule(List<int3> chunkDataPositionsToCreate)
        {
            foreach (var position in chunkDataPositionsToCreate)
            {
                ThreadSafeChunkData threadSafeChunkData = new ThreadSafeChunkData(_worldData, position);

                TerrainGenerationJob job = new TerrainGenerationJob
                {
                    chunkData = threadSafeChunkData,
                    noiseSettings = _noiseSettings,
                    dirtID = _dirtID,
                    grassID = _grassID,
                    airID = _airID
                };

                JobHandle jobHandle = job.Schedule();
                _scheduledJobs.Add(jobHandle);

                _generatedChunkData.Add(threadSafeChunkData);
            }
        }

        private void Complete()
        {
            JobHandle.CompleteAll(_scheduledJobs);

            foreach (var chunkData in _generatedChunkData)
            {
                _worldData.ExistingChunks.Add(chunkData.WorldPosition, chunkData);
                chunkData.Dispose();
            }

            _generatedChunkData.Clear();
            _scheduledJobs.Clear();
        }
    }
}