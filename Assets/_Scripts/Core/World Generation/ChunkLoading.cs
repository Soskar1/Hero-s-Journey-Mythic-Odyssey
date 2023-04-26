using System.Collections;
using UnityEngine;

namespace HerosJourney.Core.WorldGeneration
{
    public class ChunkLoading : MonoBehaviour
    {
        [SerializeField] private World _world;
        [SerializeField] private Transform _player;
        [SerializeField] private int _updateTime;
        private Vector3Int _currentPlayerChunkPosition;
        private Vector3Int _currentChunkCenter;

        private void OnEnable() => _world.OnNewChunksGenerated += StartCheckingMap;
        private void OnDisable() => _world.OnNewChunksGenerated -= StartCheckingMap;

        public void StartCheckingMap()
        {
            SetCurrentChunkCoordinates();
            StopAllCoroutines();
            StartCoroutine(TryLoadNewChunks());
        }

        private IEnumerator TryLoadNewChunks()
        {
            yield return new WaitForSeconds(_updateTime);
            if (Mathf.Abs(_currentChunkCenter.x - _player.position.x) > _world.ChunkLength ||
                Mathf.Abs(_currentChunkCenter.z - _player.position.z) > _world.ChunkLength)
            {
                _world.GenerateChunks(Vector3Int.RoundToInt(_player.position));
            }
            else
            {
                StartCoroutine(TryLoadNewChunks());
            }
        }

        private void SetCurrentChunkCoordinates()
        {
            _currentPlayerChunkPosition = WorldDataHandler.GetChunkPosition(_world.WorldData, Vector3Int.RoundToInt(_player.position));
            _currentChunkCenter.x = _currentPlayerChunkPosition.x + _world.ChunkLength / 2;
            _currentChunkCenter.z = _currentPlayerChunkPosition.z + _world.ChunkLength / 2;
        }
    }
}