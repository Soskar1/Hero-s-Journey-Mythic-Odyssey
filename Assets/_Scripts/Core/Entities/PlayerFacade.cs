using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Entities
{
    public class PlayerFacade : MonoBehaviour
    {
        private Player _player;

        [Inject]
        private void Construct(Player player)
        {
            _player = player;
        }
    }
}