using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Entities
{
    public class PlayerMovementHandler : ITickable, IFixedTickable
    {
        private Input _input;
        private Player _player;
        private Vector2 _movementInput = Vector2.zero;

        public PlayerMovementHandler(Input input, Player player)
        {
            _input = input;
            _player = player;
        }

        public void Tick()
        {
            if (_input == null)
                return;

            _movementInput = _input.GetMovementDirection();
        }

        public void FixedTick() => _player.Move(_movementInput);
    }
}