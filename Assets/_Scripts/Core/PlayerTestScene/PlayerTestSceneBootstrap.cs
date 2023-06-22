using HerosJourney.Core.Entities.PlayableCharacters;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.PlayerTestScene
{
    public class PlayerTestSceneBootstrap : IInitializable
    {
        private readonly Transform _spawnPoint;
        private readonly CinemachineFreeLook _camera;
        private readonly PlayerFacade.Factory _factory;

        public PlayerTestSceneBootstrap(PlayerFacade.Factory factory, Transform spawnPoint, CinemachineFreeLook camera)
        {
            _spawnPoint = spawnPoint;
            _factory = factory;
            _camera = camera;
        }

        public void Initialize()
        {
            SpawnPlayer();
            HideAndLockCursor();
        }

        private void SpawnPlayer()
        {
            var player = _factory.Create();
            player.transform.position = _spawnPoint.position;
            _camera.Follow = player.transform;
            _camera.LookAt = player.transform;
        }

        private void HideAndLockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}