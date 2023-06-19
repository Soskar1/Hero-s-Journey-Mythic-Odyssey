using HerosJourney.Core.Entities.PlayableCharacters;
using HerosJourney.Core.Entities;
using Cinemachine;
using UnityEngine;
using Zenject;
using System;

namespace HerosJourney.Core.Installers
{
    public class PlayerInstaller : MonoInstaller
    {
        [SerializeField] private CinemachineFreeLook _camera;
        [SerializeField] private GameObject _playerPrefab;
        [SerializeField] private Transform _spawnPoint;

        private GameObject playerInstance;

        public override void InstallBindings()
        {
            BindPlayer();
            BindInput();
            BindMovementHandler();
            BindRotationHandler();
            BindJumpHandler();
        }

        private void BindPlayer()
        {
            playerInstance = Instantiate(_playerPrefab, _spawnPoint.position, Quaternion.identity);
            IMovement movement = playerInstance.GetComponent<IMovement>();
            IRotation rotation = playerInstance.GetComponent<IRotation>();
            IJump jump = playerInstance.GetComponent<IJump>();

            Container
                .Bind<Player>()
                .AsSingle()
                .WithArguments(movement, rotation, jump);

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

        private void BindJumpHandler()
        {
            Container
                .BindInterfacesTo<PlayerJumpHandler>()
                .AsSingle()
                .WithArguments((Func<bool>)playerInstance.GetComponent<GroundCheck>().CheckForGround);
        }
    }
}