using System.Collections;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.WorldGeneration
{
    public class ChunkLoading : MonoBehaviour
    {
        [SerializeField] private int _updateTime;

        private World _world;
        private Transform _player;

        private Vector3Int _currentPlayerChunkPosition;
        private Vector3Int _currentChunkCenter;
        
        private bool _requestIsProcessed = false;

        [Inject]
        private void Construct(World world)
        {
            _world = world;
        }

        private void OnDisable()
        {
            _world.OnNewChunksInitialized -= StartCheckingMap;
            _world.OnNewChunksInitialized -= ChangeRequestStatus;
        }

        public void SetPlayer(Transform player)
        {
            _player = player;
            StartCheckingMap();

            _world.OnNewChunksInitialized += StartCheckingMap;
            _world.OnNewChunksInitialized += ChangeRequestStatus;
        }

        private void StartCheckingMap()
        {
            if (_player == null)
                return;

            SetCurrentChunkCoordinates();
            StopAllCoroutines();
            StartCoroutine(TryRequestNewChunks());
        }

        private IEnumerator TryRequestNewChunks()
        {
            yield return new WaitForSeconds(_updateTime);
            if (Mathf.Abs(_currentChunkCenter.x - _player.position.x) > _world.ChunkLength ||
                Mathf.Abs(_currentChunkCenter.z - _player.position.z) > _world.ChunkLength)
            {
                if (!_requestIsProcessed)
                {
                    _world.GenerateChunksRequest(Vector3Int.RoundToInt(_player.position));
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
            _currentChunkCenter.z = _currentPlayerChunkPosition.z + _world.ChunkLength / 2;
        }

        private void ChangeRequestStatus() => _requestIsProcessed = false;
    }
}