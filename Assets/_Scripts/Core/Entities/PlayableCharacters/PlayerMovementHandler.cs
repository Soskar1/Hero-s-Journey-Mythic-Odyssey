using UnityEngine;
using Zenject;

namespace HerosJourney.Core.Entities.PlayableCharacters
{
    public class PlayerMovementHandler : ITickable, IFixedTickable
    {
        private readonly Player _player;
        private readonly PlayerInputHandler _inputHandler;
        private readonly CollisionClimbing _collisionClimbing;
        private readonly Rigidbody _rigidbody;
        private Vector3 _targetDirection;

        private Vector3 _lastVelocity;

        public PlayerMovementHandler(Player player, PlayerInputHandler inputHandler, CollisionClimbing collisionClimbing, Rigidbody rigidbody)
        {
            _player = player;
            _inputHandler = inputHandler;
            _collisionClimbing = collisionClimbing;
            _rigidbody = rigidbody;
        }

        public void Tick() => _targetDirection = Quaternion.Euler(0.0f, _inputHandler.RotY, 0.0f) * Vector3.forward;

        public void FixedTick()
        {
            Vector3 movement = _inputHandler.MovementInput.magnitude == 0 ? Vector3.zero : _targetDirection;
            _player.Move(movement);

            if (movement != Vector3.zero)
                _collisionClimbing.StepClimb(_lastVelocity);

            _lastVelocity = _rigidbody.velocity;
        }
    }
}