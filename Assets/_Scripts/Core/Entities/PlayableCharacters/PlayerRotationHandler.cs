using Zenject;

namespace HerosJourney.Core.Entities.PlayableCharacters
{
    public class PlayerRotationHandler : ITickable
    {
        private readonly Player _player;
        private readonly PlayerInputHandler _inputHandler;

        public PlayerRotationHandler(Player player, PlayerInputHandler inputHandler)
        {
            _player = player;
            _inputHandler = inputHandler;
        }

        public void Tick()
        {
            if (_inputHandler.MovementInput.magnitude > 0)
                _player.Rotate(_inputHandler.RotY);
        }
    }
}