using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Installers
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            BindMainCamera();
        }

        private void BindMainCamera()
        {
            Container
                .Bind<Camera>()
                .FromInstance(Camera.main)
                .AsSingle();
        }
    }
}