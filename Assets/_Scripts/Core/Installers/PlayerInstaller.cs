using Cinemachine;
using HerosJourney.Core.Entities;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private CinemachineFreeLook _camera;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Transform _spawnPoint;

        public override void InstallBindings()
        {
            BindPlayer();
            BindInput();
            BindMovementHandler();
            BindRotationHandler();
        }
      
        private void BindPlayer()
        {
            GameObject playerInstance = Instantiate(_playerPrefab, _spawnPoint.position, Quaternion.identity);
            IMovement movement = playerInstance.GetComponent<IMovement>();
            IRotation rotation = playerInstance.GetComponent<IRotation>();

            Container
                .Bind<Player>()
                .AsSingle()
                .WithArguments(movement, rotation);

            _camera.Follow = playerInstance.transform;
            _camera.LookAt = playerInstance.transform;
        }

        private void BindInput()
        {
            Container
                .BindInterfacesAndSelfTo<Input>()
                .AsSingle()
                .NonLazy();

            Container
                .Bind<PlayerInputHandler>()
                .AsSingle();
        }

        private void BindMovementHandler()
        {
            Container
                .BindInterfacesTo<PlayerMovementHandler>()
                .AsSingle();
        }

        private void BindRotationHandler()
        {
            Container
                .BindInterfacesTo<PlayerRotationHandler>()
                .AsSingle();
        }
    }
}