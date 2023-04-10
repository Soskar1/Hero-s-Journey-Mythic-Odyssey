using Unity.Jobs;
using UnityEngine;

namespace HerosJourney.Core.Entities
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(ThirdPersonLook))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private PlayerInput _input;
        [SerializeField] private ThirdPersonLook _thirdPerson;
        private IMovement _movement;
        private Camera _camera;

        private Vector2 _movementInput;
        private Vector3 _targetDirection;
        private float _targetRotation;

        private void Awake()
        {
            _movement = GetComponent<IMovement>();
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            _input.Initialize();
            _input.Enable();
        }

        private void Update()
        {
            _movementInput = _input.GetMovementDirection();
            _targetRotation = Mathf.Atan2(_movementInput.x, _movementInput.y) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            _thirdPerson.Rotate(_targetRotation);

            _targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        }

        private void FixedUpdate()
        {
            if (_movementInput.magnitude > 0)
                _movement.Move(_targetDirection);
        }
    }
}