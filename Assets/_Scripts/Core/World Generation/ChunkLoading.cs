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
        
        private bool _requestIsProcessed = false;

        private void OnEnable()
        {
            _world.OnNewChunksInitialized += StartCheckingMap;
            _world.OnNewChunksInitialized += ChangeRequestStatus;
        }
        private void OnDisable()
        {
            _world.OnNewChunksInitialized -= StartCheckingMap;
            _world.OnNewChunksInitialized -= ChangeRequestStatus;
        }

        public void StartCheckingMap()
        {
            SetCurrentChunkCoordinates();
            StopAllCoroutines();
            StartCoroutine(TryRequestNewChunks());
        }

        private IEnumerator TryRequestNewChunks()
        {
            yield return new WaitForSeconds(_updateTime);
            if (Mathf.Abs(_currentChunkCenter.x - _player.position.x) > _world.ChunkLength ||
                Mathf.Abs(_currentChunkCenter.y - _player.position.y) > _world.ChunkHeight ||
                Mathf.Abs(_currentChunkCenter.z - _player.position.z) > _world.ChunkLength)
            {
                if (!_requestIsProcessed)
                {
                    _world.GenerateChunks(Vector3Int.RoundToInt(_player.position));
                    _requestIsProcessed = true;
                }
            }
            else
            {
                StartCoroutine(TryRequestNewChunks());
            }
        }

        private void SetCurrentChunkCoordinates()
        {
            _currentPlayerChunkPosition = WorldDataHandler.GetChunkPosition(_world.WorldData, Vector3Int.RoundToInt(_player.position));
            _currentChunkCenter.x = _currentPlayerChunkPosition.x + _world.ChunkLength / 2;
            _currentChunkCenter.y = _currentPlayerChunkPosition.y + _world.ChunkHeight / 2;
            _currentChunkCenter.z = _currentPlayerChunkPosition.z + _world.ChunkLength / 2;
        }

        private void ChangeRequestStatus() => _requestIsProcessed = false;
    }
}