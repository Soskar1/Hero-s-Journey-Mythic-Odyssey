using UnityEngine;

namespace HerosJourney.Core.Entities
{
    [RequireComponent(typeof(PlayerInput))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerInput _input;
        private IMovement _movement;

        private void Awake() => _movement = GetComponent<IMovement>();

        private void OnEnable()
        {
            _input.Initialize();
            _input.Enable();
        }

        private void FixedUpdate()
        {
            _movement.Move(_input.GetMovementDirection());
        }
    }
}