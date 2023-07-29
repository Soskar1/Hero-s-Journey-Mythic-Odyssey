using Zenject;
using System;
using UnityEngine;

namespace HerosJourney.StructureBuilder.Actor
{
    public class Input : IInitializable, IDisposable
    {
        public Controls Controls { get; private set; }

        public Input() => Controls = new Controls();

        public void Initialize() => Enable();
        public void Dispose() => Disable();

        public void Enable() => Controls.Enable();
        public void Disable() => Controls.Disable();

        public Vector2 MovementInput => Controls.Builder.Movement.ReadValue<Vector2>();
        public Vector2 DeltaMouse => Controls.Builder.DeltaMouse.ReadValue<Vector2>();
    }
}