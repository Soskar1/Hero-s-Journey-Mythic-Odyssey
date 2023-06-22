using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Entities.PlayableCharacters
{
    public class PlayerFacade : MonoBehaviour
    {
        private Player _player;

        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }

        public class Factory : PlaceholderFactory<PlayerFacade> { }
    }
}