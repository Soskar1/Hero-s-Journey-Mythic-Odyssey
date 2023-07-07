using Zenject;
using System;

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
    }
}