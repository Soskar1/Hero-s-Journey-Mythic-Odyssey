using Cinemachine;
using HerosJourney.Core.Entities;
using HerosJourney.Core.Entities.PlayableCharacters;
using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class PlayerTestSceneInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _playerFacade;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private CinemachineFreeLook _camera;

        public override void InstallBindings()
        {
            BindCameras();
            BindPlayerFacadeFactory();
            BindSceneBootstrap();
        }

        private void BindCameras()
        {
            Container
                .Bind<Camera>()
                .FromInstance(Camera.main)
                .AsSingle();

            Container
                .Bind<CinemachineFreeLook>()
                .FromInstance(_camera)
                .AsSingle();
        }

        private void BindPlayerFacadeFactory()
        {
            Container
                .BindFactory<PlayerFacade, PlayerFacade.Factory>()
                .FromSubContainerResolve()
                .ByNewPrefabInstaller<PlayerInstaller>(_playerFacade);
        }

        private void BindSceneBootstrap()
        {
            Container
                .BindInterfacesTo<PlayerTestSceneBootstrap>()
                .AsSingle()
                .WithArguments(_spawnPoint);
        }
    }
}