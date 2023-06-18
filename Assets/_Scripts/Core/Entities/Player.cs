using UnityEngine;

namespace HerosJourney.Core.Entities
{
    public class Player
    {
        private IMovement _movement;

        public Player(IMovement movement)
        {
            _movement = movement;
        }

        public void Move(Vector3 direction) => _movement.Move(direction);
    }
}