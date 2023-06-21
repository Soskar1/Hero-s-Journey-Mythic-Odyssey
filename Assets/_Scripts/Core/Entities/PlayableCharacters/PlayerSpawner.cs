using Cinemachine;
using UnityEngine;

namespace HerosJourney.Core.Entities.PlayableCharacters
{
    public class PlayerSpawner
    {
        private readonly PlayerFacade.Factory _playerFactory;
        private readonly CinemachineFreeLook _camera;
        private readonly Transform _spawnPosition;

        public PlayerSpawner(PlayerFacade.Factory playerFactory, CinemachineFreeLook camera, Transform spawnPosition)
        {
            _playerFactory = playerFactory;
            _camera = camera;
            _spawnPosition = spawnPosition;
        }

        public PlayerFacade SpawnPlayer()
        {
            PlayerFacade player = _playerFactory.Create();
            player.transform.position = _spawnPosition.position;

            _camera.Follow = player.transform;
            _camera.LookAt = player.transform;

            return player;
        }
    }
}