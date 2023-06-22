using UnityEngine;

namespace HerosJourney.Core.Entities.PlayableCharacters
{
    public class Player
    {
        private readonly IMovement _movement;
        private readonly IRotation _rotation;
        private readonly IJump _jumping;

        public Player(IMovement movement, IRotation rotation, IJump jumping)
        {
            _movement = movement;
            _rotation = rotation;
            _jumping = jumping;
        }

        public void Move(Vector3 direction) => _movement.Move(direction);
        public void Rotate(float rotY) => _rotation.Rotate(rotY);
        public void Jump() => _jumping.Jump();
    }
}