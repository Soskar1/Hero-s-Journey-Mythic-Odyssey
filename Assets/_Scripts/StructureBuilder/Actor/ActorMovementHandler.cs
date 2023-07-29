using UnityEngine;
using Zenject;

namespace HerosJourney.StructureBuilder.Actor
{
    public class ActorMovementHandler : ITickable
    {
        private readonly Actor _actor;
        private readonly ActorInputHandler _inputHandler;

        public ActorMovementHandler(Actor actor, ActorInputHandler inputHandler)
        {
            _actor = actor;
            _inputHandler = inputHandler;
        }

        public void Tick()
        {
            
        }
    }
}