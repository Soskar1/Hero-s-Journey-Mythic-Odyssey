using UnityEngine;

namespace HerosJourney.Core.Entities
{
    public class PlayerInputHandler
    {
        private readonly Input _input;
        private readonly Camera _camera;

        public Vector2 MovementInput => _input.MovementInput;
        public float RotY => Mathf.Atan2(MovementInput.x, MovementInput.y) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;

        public PlayerInputHandler(Input input, Camera camera)
        {
            _input = input;
            _camera = camera;
        }
    }
}