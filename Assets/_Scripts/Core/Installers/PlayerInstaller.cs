using HerosJourney.Core.Entities.PlayableCharacters;
using HerosJourney.Core.Entities;
using Zenject;
using System;

namespace HerosJourney.Core.Installers
{
    public class PlayerInstaller : Installer<PlayerInstaller>
    {
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
            Container
                .BindInterfacesAndSelfTo<Player>()
                .AsSingle();
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
                .AsSingle();
        }
    }
}