using UnityEngine;

namespace HerosJourney.Core.Entities
{
    public class Player
    {
        private readonly IMovement _movement;
        private readonly IRotation _rotation;

        public Player(IMovement movement, IRotation rotation)
        {
            _movement = movement;
            _rotation = rotation;
        }

        public void Move(Vector3 direction) => _movement.Move(direction);
        public void Rotate(float rotY) => _rotation.Rotate(rotY);
    }
}