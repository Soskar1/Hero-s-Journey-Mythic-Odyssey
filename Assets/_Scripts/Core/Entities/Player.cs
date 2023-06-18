using UnityEngine;
using UnityEngine.InputSystem;

namespace HerosJourney.Core.Entities
{
    [RequireComponent(typeof(ThirdPersonLook))]
    [RequireComponent(typeof(GroundCheck))]
    [RequireComponent(typeof(Jumping))]
    [RequireComponent(typeof(CollisionClimbing))]
    public class Player : MonoBehaviour
    {
        private Input _input;
        [SerializeField] private ThirdPersonLook _thirdPerson;
        [SerializeField] private GroundCheck _groundCheck;
        [SerializeField] private Jumping _jumping;
        [SerializeField] private CollisionClimbing _collisionClimbing;
        [SerializeField] private Rigidbody _rigidbody;
        private IMovement _movement;
        private Camera _camera;

        private Vector2 _movementInput;
        private Vector3 _targetDirection;
        private Vector3 _lastVelocity;

        private void Awake()
        {
            _movement = GetComponent<IMovement>();
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            _input = new Input();
            _input.Enable();

            _input.Controls.Player.Jump.performed += Jump;
        }

        private void OnDisable()
        {
            _input.Controls.Player.Jump.performed -= Jump;
            _input.Disable();
        }

        private void Update()
        {
            _movementInput = _input.GetMovementDirection();
            float targetRotation = Mathf.Atan2(_movementInput.x, _movementInput.y) * Mathf.Rad2Deg + _camera.transform.eulerAngles.y;
            _thirdPerson.Rotate(targetRotation);

            _targetDirection = Quaternion.Euler(0.0f, targetRotation, 0.0f) * Vector3.forward;
        }

        private void FixedUpdate()
        {
            if (_movementInput.magnitude > 0)
            {
                _movement.Move(_targetDirection);
                _collisionClimbing.StepClimb(_lastVelocity);
            }
            else
            {
                _movement.Move(Vector3.zero);
            }

            _lastVelocity = _rigidbody.velocity;
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (context.performed)
                if (_groundCheck.CheckForGround())
                    _jumping.Jump();
        }
    }
}