using Cinemachine;
using HerosJourney.Core.Entities.PlayableCharacters;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class SceneInstaller : MonoInstaller
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
                .Bind<PlayerSpawner>()
                .AsSingle()
                .WithArguments(_spawnPoint);
        }
    }
}