using UnityEngine;

namespace HerosJourney.StructureBuilder.Actor
{
    public class ActorInputHandler
    {
        private readonly Input _input;

        public ActorInputHandler(Input input) => _input = input;

        public Vector2 MovementInput => _input.MovementInput;
        public Vector2 DeltaMouse => _input.DeltaMouse;
    }
}