using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Entities.PlayableCharacters
{
    public class PlayerMovementHandler : ITickable, IFixedTickable
    {
        private readonly Player _player;
        private readonly PlayerInputHandler _inputHandler;
        private Vector3 _targetDirection;

        public PlayerMovementHandler(Player player, PlayerInputHandler inputHandler)
        {
            _player = player;
            _inputHandler = inputHandler;
        }

        public void Tick() => _targetDirection = Quaternion.Euler(0.0f, _inputHandler.RotY, 0.0f) * Vector3.forward;

        public void FixedTick()
        {
            Vector3 movement = _inputHandler.MovementInput.magnitude == 0 ? Vector3.zero : _targetDirection;
            _player.Move(movement);
        }
    }
}