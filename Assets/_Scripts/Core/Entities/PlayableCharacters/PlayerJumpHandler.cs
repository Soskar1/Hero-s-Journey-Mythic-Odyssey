using UnityEngine.InputSystem;
using Zenject;
using System;

namespace HerosJourney.Core.Entities.PlayableCharacters
{
    public class PlayerJumpHandler : IInitializable, IDisposable
    {
        private readonly Input _input;
        private readonly Player _player;

        public PlayerJumpHandler(Input input, Player player)
        {
            _input = input;
            _player = player;
        }

        public void Initialize() => _input.Controls.Player.Jump.performed += Jump;
        public void Dispose() => _input.Controls.Player.Jump.performed -= Jump;

        private void Jump(InputAction.CallbackContext context) => _player.Jump();
    }
}