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
        [SerializeField] private float _step;
        [SerializeField] private float _maxStepHeight;
        [SerializeField] private float _lowerRaycastLength;
        [SerializeField] private float _upperRaycastLength;

        [SerializeField] private List<float> _angles = new List<float>();

        private void Awake() => _upperStepRay.position = new Vector3(_upperStepRay.position.x, _lowerStepRay.position.y + _maxStepHeight, _upperStepRay.position.z);

        public void StepClimb()
        {
            for (int index = 0; index < _angles.Count; ++index)
            {
                if (CheckForCollision(_angles[index]))
                {
                    _rigidbody.position += new Vector3(0f, _step, 0f);
                    return;
                }
            }
        }

        private bool CheckForCollision(float rotY)
        {
            if (Physics.Raycast(_lowerStepRay.transform.position, Quaternion.Euler(0, rotY, 0) * transform.forward, _lowerRaycastLength, _collisionLayerMask))
                if (!Physics.Raycast(_upperStepRay.transform.position, Quaternion.Euler(0, rotY, 0) * transform.forward, _upperRaycastLength, _collisionLayerMask))
                    return true;

            return false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            foreach (float angle in _angles)
                Gizmos.DrawLine(_lowerStepRay.position, _lowerStepRay.position + Quaternion.Euler(0, angle, 0) * transform.forward * _lowerRaycastLength);

            Vector3 upperStepRayPosition = new Vector3(_upperStepRay.position.x, _lowerStepRay.position.y + _maxStepHeight, _upperStepRay.position.z);

            Gizmos.color = Color.green;

            foreach (float angle in _angles)
                Gizmos.DrawLine(upperStepRayPosition, upperStepRayPosition + Quaternion.Euler(0, angle, 0) * transform.forward * _lowerRaycastLength);
        }
    }
}