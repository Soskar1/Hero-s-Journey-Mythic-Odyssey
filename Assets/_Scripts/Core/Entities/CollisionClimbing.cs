using UnityEngine;
using System.Collections.Generic;

namespace HerosJourney.Core.Entities
{
    [RequireComponent(typeof(Rigidbody))]
    public class CollisionClimbing : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private Transform _upperStepRay;
        [SerializeField] private Transform _lowerStepRay;
        [SerializeField] private LayerMask _collisionLayerMask;
        [SerializeField] private float _stepOffset = 0.01f;
        [SerializeField] private float _maxStepHeight;
        [SerializeField] private float _lowerRaycastMaxLength;
        [SerializeField] private float _upperRaycastMaxLength;

        [SerializeField] private List<float> _angles = new List<float>();

        private void Awake() => _upperStepRay.position = new Vector3(_upperStepRay.position.x, _lowerStepRay.position.y + _maxStepHeight, _upperStepRay.position.z);

        public void StepClimb(Vector3 lastVelocity)
        {
            for (int index = 0; index < _angles.Count; ++index)
            {
                float heightDifference;
                if (CheckForCollision(_angles[index], out heightDifference))
                {
                    _rigidbody.position += new Vector3(0f, heightDifference + _stepOffset, 0f);
                    _rigidbody.velocity = lastVelocity;
                    return;
                }
            }
        }

        private bool CheckForCollision(float rotY, out float heightDifference)
        {
            heightDifference = 0;
            Quaternion rotation = Quaternion.Euler(0, rotY, 0);

            if (!Physics.Raycast(_lowerStepRay.transform.position, rotation * transform.forward, _lowerRaycastMaxLength, _collisionLayerMask))
                return false;

            if (Physics.Raycast(_upperStepRay.transform.position, rotation * transform.forward, _upperRaycastMaxLength, _collisionLayerMask))
                return false;

            RaycastHit hitInfo;
            Vector3 origin = _upperStepRay.transform.position + rotation * transform.forward * _upperRaycastMaxLength;

            if (!Physics.Raycast(new Ray(origin, Vector3.down), out hitInfo, _maxStepHeight, _collisionLayerMask))
                return false;

            heightDifference = hitInfo.point.y - _lowerStepRay.position.y;

            return true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            foreach (float angle in _angles)
                Gizmos.DrawLine(_lowerStepRay.position, _lowerStepRay.position + Quaternion.Euler(0, angle, 0) * transform.forward * _lowerRaycastMaxLength);

            Vector3 upperStepRayPosition = new Vector3(_upperStepRay.position.x, _lowerStepRay.position.y + _maxStepHeight, _upperStepRay.position.z);

            Gizmos.color = Color.green;

            foreach (float angle in _angles)
                Gizmos.DrawLine(upperStepRayPosition, upperStepRayPosition + Quaternion.Euler(0, angle, 0) * transform.forward * _lowerRaycastMaxLength);
        }
    }
}