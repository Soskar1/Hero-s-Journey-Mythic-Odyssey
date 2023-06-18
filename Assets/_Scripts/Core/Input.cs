using UnityEngine;

namespace HerosJourney.Core
{
    public class Input
    {
        public Controls Controls { get; private set; }

        public Input() => Controls = new Controls();

        public void Enable() => Controls.Enable();
        public void Disable() => Controls.Disable();

        public Vector2 GetMovementDirection() => Controls.Player.Movement.ReadValue<Vector2>();
    }
}