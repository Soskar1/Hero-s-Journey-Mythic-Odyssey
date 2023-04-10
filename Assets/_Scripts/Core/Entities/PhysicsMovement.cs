using UnityEngine;

namespace HerosJourney.Core.Entities
{
    public class PhysicsMovement : MonoBehaviour, IMovement
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private float _acceleration;
        [SerializeField] private float _decceleration;
        [SerializeField] private float _velocityPower;

        private const float EPSILON = 0.01f;

        public void Move(Vector3 direction)
        {
            Vector3 targetVelocity = new Vector3(direction.x * _maxSpeed, 0f, direction.z * _maxSpeed);
            Vector3 velocityDifference = targetVelocity - _rigidbody.velocity;
            float accelerationRate = (Mathf.Abs(targetVelocity.magnitude) > EPSILON) ? _acceleration : _decceleration;
            float movementX = Mathf.Pow(Mathf.Abs(velocityDifference.x) * accelerationRate, _velocityPower) * Mathf.Sign(velocityDifference.x);
            float movementZ = Mathf.Pow(Mathf.Abs(velocityDifference.z) * accelerationRate, _velocityPower) * Mathf.Sign(velocityDifference.z);

            _rigidbody.AddForce(new Vector3(movementX, _rigidbody.velocity.y, movementZ));
        }
    }
}