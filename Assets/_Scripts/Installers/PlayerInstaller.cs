using HerosJourney.Core.Entities;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Rigidbody _rigidbody;

    public override void InstallBindings()
    {
        Container.Bind<Player>().AsSingle().WithArguments(new PhysicsMovement(_rigidbody));
        Container.BindInterfacesAndSelfTo<HerosJourney.Core.Input>().AsSingle().NonLazy();

        Container.BindInterfacesTo<PlayerMovementHandler>().AsSingle();
    }
}