using UnityEngine;

namespace HerosJourney.Core.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class CollisionClimbing : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _upperStepRay;
        [SerializeField] private Transform _lowerStepRay;
        [SerializeField] private LayerMask _collisionLayerMask;
        [SerializeField] private float _stepSmooth;
        [SerializeField] private float _stepHeight;
        [SerializeField] private float _lowerRaycastLength;
        [SerializeField] private float _upperRaycastLength;

        private void Awake() => _upperStepRay.position = new Vector3(_upperStepRay.position.x, _lowerStepRay.position.y + _stepHeight, _upperStepRay.position.z);

        public void StepClimb()
        {
            if (CheckForCollision(0.0f) || CheckForCollision(45f) || CheckForCollision(-45f))
                _rigidbody.position += new Vector3(0f, _stepSmooth, 0f);
        }

        private bool CheckForCollision(float rotY)
        {
            if (Physics.Raycast(_lowerStepRay.transform.position, Quaternion.Euler(0, rotY, 0) * transform.forward, _lowerRaycastLength, _collisionLayerMask))
                if (!Physics.Raycast(_upperStepRay.transform.position, transform.forward, _upperRaycastLength, _collisionLayerMask))
                    return true;

            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_lowerStepRay.position, _lowerStepRay.position + transform.forward * _lowerRaycastLength);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(new Vector3(_upperStepRay.position.x, _lowerStepRay.position.y + _stepHeight, _upperStepRay.position.z), new Vector3(_upperStepRay.position.x, _lowerStepRay.position.y + _stepHeight, _upperStepRay.position.z) + transform.forward * _lowerRaycastLength);
        }
    }
}