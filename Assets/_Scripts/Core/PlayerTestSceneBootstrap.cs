using HerosJourney.Core.Entities.PlayableCharacters;
using UnityEngine;
using Zenject;

namespace HerosJourney.Core
{
    public class PlayerTestSceneBootstrap : IInitializable
    {
        private readonly Transform _spawnPoint;
        private readonly PlayerFacade.Factory _factory;

        public PlayerTestSceneBootstrap(PlayerFacade.Factory factory, Transform spawnPoint)
        {
            _spawnPoint = spawnPoint;
            _factory = factory;
        }

        public void Initialize()
        {
            var player = _factory.Create();
            player.transform.position = _spawnPoint.position;
        }
    }
}