using UnityEngine;
using Zenject;

namespace HerosJourney.StructureBuilder.Installers
{
    public class BuilderInstaller : MonoInstaller
    {
        [SerializeField] private Vector3Int _size;
        [SerializeField] private int _groundID;

        public override void InstallBindings()
        {
            Container
                .Bind<StructureData>()
                .AsSingle()
                .WithArguments(_size)
                .NonLazy();

            Container
                .BindInterfacesAndSelfTo<StructureBuilder>()
                .AsSingle()
                .WithArguments(_groundID)
                .NonLazy();
        }
    }
}